using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Qlarissa.Application.Interfaces.ExternalAPI;
using Qlarissa.Domain.Securities.MarketData;
using Qlarissa.Infrastructure.PyFinance.Options;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace Qlarissa.Infrastructure.PyFinance;

public sealed class YahooLivePriceService : BackgroundService, ILivePriceService
{
    private const string YahooWebSocketUrl = "wss://streamer.finance.yahoo.com/?version=2";

    private const int YahooHeartbeatSeconds = 15;

    private readonly ILogger<YahooLivePriceService> _logger;
    private readonly LivePriceOptions _options;

    private readonly object _stateLock = new();

    // consumerId -> symbols
    private readonly Dictionary<string, HashSet<string>> _consumerSubscriptions = new(StringComparer.Ordinal);

    // symbol -> number of active analysis locks
    private readonly Dictionary<string, int> _analysisLockCounts = new(StringComparer.OrdinalIgnoreCase);

    // symbol -> latest received price
    private readonly Dictionary<string, LivePrice> _latestPrices = new(StringComparer.OrdinalIgnoreCase);

    // ------------------------------------------------------------
    // Signals the Yahoo worker when subscription state changes.
    // Only one signal needs to be queued at a time.
    // ------------------------------------------------------------

    private readonly Channel<bool> _subscriptionChanges =
        Channel.CreateBounded<bool>(
            new BoundedChannelOptions(1)
            {
                SingleWriter = false,
                SingleReader = true,
                FullMode = BoundedChannelFullMode.DropWrite
            });

    public YahooLivePriceService(ILogger<YahooLivePriceService> logger, IOptions<LivePriceOptions> options)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));

        if (_options.BatchInterval <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(options), "BatchInterval must be greater than zero.");
        }

        if (_options.InitialReconnectDelay <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(options), "InitialReconnectDelay must be greater than zero.");
        }

        if (_options.MaximumReconnectDelay < _options.InitialReconnectDelay)
        {
            throw new ArgumentOutOfRangeException(nameof(options), "MaximumReconnectDelay must be greater than or equal to InitialReconnectDelay.");
        }
    }

    public Task SubscribeAsync(string consumerId, IEnumerable<string> symbols, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(consumerId);
        cancellationToken.ThrowIfCancellationRequested();

        if (!symbols.Any())
            return Task.CompletedTask;

        lock (_stateLock)
        {
            if (!_consumerSubscriptions.TryGetValue(consumerId, out var subscriptions))
            {
                subscriptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                _consumerSubscriptions[consumerId] = subscriptions;
            }

            foreach (var symbol in symbols)
            {
                subscriptions.Add(symbol);
            }
        }

        SignalSubscriptionChange();

        return Task.CompletedTask;
    }

    public Task UnsubscribeAsync(string consumerId, IEnumerable<string> symbols, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(consumerId);
        cancellationToken.ThrowIfCancellationRequested();

        if (!symbols.Any())
            return Task.CompletedTask;

        lock (_stateLock)
        {
            if (!_consumerSubscriptions.TryGetValue(
                    consumerId,
                    out var subscriptions))
            {
                return Task.CompletedTask;
            }

            foreach (var symbol in symbols)
            {
                subscriptions.Remove(symbol);
            }

            if (subscriptions.Count == 0)
            {
                _consumerSubscriptions.Remove(consumerId);
            }
        }

        SignalSubscriptionChange();

        return Task.CompletedTask;
    }

    public Task RemoveConsumerAsync(string consumerId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(consumerId);
        cancellationToken.ThrowIfCancellationRequested();

        lock (_stateLock)
        {
            _consumerSubscriptions.Remove(consumerId);
        }

        SignalSubscriptionChange();
        return Task.CompletedTask;
    }

    public IReadOnlyList<LivePrice> GetPrices(IEnumerable<string> symbols)
    {
        if (!symbols.Any())
            return [];

        lock (_stateLock)
        {
            var result = new List<LivePrice>(symbols.Count());
            foreach (var symbol in symbols)
            {
                if (_latestPrices.TryGetValue(symbol, out var price))
                {
                    result.Add(price);
                }
            }

            return result;
        }
    }

    public Task<SubscriptionLock> AcquireLockAsync(IEnumerable<string> symbols, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!symbols.Any())
        {
            return Task.FromResult(new SubscriptionLock(Array.Empty<string>(), _ => ValueTask.CompletedTask));
        }

        lock (_stateLock)
        {
            foreach (var symbol in symbols)
            {
                _analysisLockCounts.TryGetValue(symbol, out var count);
                _analysisLockCounts[symbol] = count + 1;
            }
        }

        // Analysis locks also cause Yahoo subscriptions to become active if they weren't already active.
        SignalSubscriptionChange();
        var subscriptionLock = new SubscriptionLock((IReadOnlyCollection<string>)symbols, ReleaseAnalysisLocksAsync);

        return Task.FromResult(subscriptionLock);
    }

    public IReadOnlyDictionary<string, IReadOnlyCollection<string>> GetConsumerSubscriptions()
    {
        lock (_stateLock)
        {
            return _consumerSubscriptions.ToDictionary(
                pair => pair.Key,
                pair => (IReadOnlyCollection<string>)pair.Value.ToArray());
        }
    }

    // BackgroundService
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Yahoo live-price service started.");

        try
        {
            await RunYahooWorkerAsync(stoppingToken);
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            // Normal application shutdown.
        }
        finally
        {
            _logger.LogInformation("Yahoo live-price service stopped.");
        }
    }

    private async Task RunYahooWorkerAsync(CancellationToken stoppingToken)
    {
        var reconnectDelay = _options.InitialReconnectDelay;

        while (!stoppingToken.IsCancellationRequested)
        {
            if (!HasEffectiveSubscriptions())
            {
                try
                {
                    await _subscriptionChanges.Reader.WaitToReadAsync(stoppingToken);
                    DrainSubscriptionSignals();
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    return;
                }

                continue;
            }

            try
            {
                await RunYahooConnectionAsync(stoppingToken);

                // A clean return generally means the connection was deliberately stopped because there are no longer any required subscriptions.
                reconnectDelay = _options.InitialReconnectDelay;
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Yahoo WebSocket connection failed. Reconnecting in {Delay}.", reconnectDelay);

                try
                {
                    await Task.Delay(reconnectDelay,stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    return;
                }

                reconnectDelay = TimeSpan.FromMilliseconds(Math.Min(reconnectDelay.TotalMilliseconds * 2, _options.MaximumReconnectDelay.TotalMilliseconds));
            }
        }
    }

    private async Task RunYahooConnectionAsync(CancellationToken stoppingToken)
    {
        using var socket = new ClientWebSocket();
        await socket.ConnectAsync(new Uri(YahooWebSocketUrl), stoppingToken);
        _logger.LogInformation("Connected to Yahoo Finance WebSocket.");
        var currentSubscriptions = GetEffectiveSubscriptions();

        if (currentSubscriptions.Count == 0)
            return;

        await SendSubscribeAsync(socket, currentSubscriptions, stoppingToken);

        // Exactly one receive operation is active for this socket.
        var receiveTask = ReceiveLoopAsync(socket, stoppingToken);
        var heartbeatTask = Task.Delay(TimeSpan.FromSeconds(YahooHeartbeatSeconds), stoppingToken);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var subscriptionChangeTask = WaitForSubscriptionChangeAsync(stoppingToken);
                var completedTask = await Task.WhenAny(receiveTask, subscriptionChangeTask, heartbeatTask);

                if (completedTask == receiveTask)
                {
                    // ReceiveLoop returns when the connection closes or throws when the connection fails.
                    await receiveTask;
                    return;
                }

                if (completedTask == subscriptionChangeTask)
                {
                    DrainSubscriptionSignals();
                    var desiredSubscriptions = GetEffectiveSubscriptions();

                    if (desiredSubscriptions.Count == 0)
                    {
                        // No consumer or analysis requires anything. Stop the Yahoo connection.
                        socket.Abort();
                        return;
                    }

                    var added = desiredSubscriptions.Except(currentSubscriptions, StringComparer.OrdinalIgnoreCase).ToArray();
                    var removed = currentSubscriptions.Except(desiredSubscriptions, StringComparer.OrdinalIgnoreCase).ToArray();

                    if (removed.Length > 0)
                    {
                        await SendUnsubscribeAsync(socket, removed, stoppingToken);
                    }

                    if (added.Length > 0)
                    {
                        await SendSubscribeAsync(socket,added,stoppingToken);
                    }

                    currentSubscriptions = desiredSubscriptions;

                    continue;
                }

                if (completedTask == heartbeatTask)
                {
                    if (currentSubscriptions.Count > 0)
                    {
                        await SendSubscribeAsync(socket, currentSubscriptions, stoppingToken);
                    }

                    heartbeatTask = Task.Delay(TimeSpan.FromSeconds(YahooHeartbeatSeconds), stoppingToken);
                }
            }
        }
        finally
        {
            socket.Abort();

            try
            {
                await receiveTask;
            }
            catch
            {
                // The connection is already being torn down.
            }
        }
    }

    private async Task ReceiveLoopAsync(ClientWebSocket socket, CancellationToken cancellationToken)
    {
        var buffer = new byte[16 * 1024];

        while (!cancellationToken.IsCancellationRequested && socket.State == WebSocketState.Open)
        {
            using var stream = new MemoryStream();
            WebSocketReceiveResult result;

            do
            {
                result = await socket.ReceiveAsync(buffer, cancellationToken);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    throw new WebSocketException("Yahoo closed the WebSocket.");
                }

                stream.Write(buffer, 0, result.Count);

            } while (!result.EndOfMessage);

            if (result.MessageType != WebSocketMessageType.Text)
            {
                continue;
            }

            ProcessYahooMessage(stream.GetBuffer(), checked((int)stream.Length));
        }
    }

    private void ProcessYahooMessage(byte[] buffer, int length)
    {
        try
        {
            using var json = JsonDocument.Parse(new ReadOnlyMemory<byte>(buffer, 0, length));

            if (!json.RootElement.TryGetProperty("message", out var messageElement))
                return;

            var base64Message = messageElement.GetString();

            if (string.IsNullOrWhiteSpace(base64Message))
                return;

            var protobufBytes = Convert.FromBase64String(base64Message);
            var data = YahooMarketData.PricingData.Parser.ParseFrom(protobufBytes);

            if (string.IsNullOrWhiteSpace(data.Id))
                return;

            var symbol = data.Id.Trim().ToUpperInvariant();
            var timestamp = ConvertYahooTimestamp(data.Time);

            var livePrice = new LivePrice
            {
                Symbol = symbol,
                Timestamp = timestamp,
                Price = data.Price,
                DayHigh = data.DayHigh,
                DayLow = data.DayLow
            };

            lock (_stateLock)
            {
                _latestPrices[symbol] = livePrice;
            }
        }
        catch (Exception ex)
        {
            // A malformed individual Yahoo message should not terminate the entire WebSocket connection.
            _logger.LogWarning(ex, "Failed to decode a Yahoo Finance WebSocket message.");
        }
    }

    private static DateTime ConvertYahooTimestamp(long timestamp)
    {
        if (timestamp <= 0)
            return DateTime.UtcNow;

        return DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime;
    }

    // Yahoo send operations
    // These are ONLY called by RunYahooConnectionAsync, ensuring that ClientWebSocket never receives concurrent sends.
    private static async Task SendSubscribeAsync(ClientWebSocket socket, IReadOnlyCollection<string> symbols, CancellationToken cancellationToken)
    {
        if (symbols.Count == 0)
            return;

        var message = JsonSerializer.Serialize(new { subscribe = symbols });
        await SendTextAsync(socket, message, cancellationToken);
    }

    private static async Task SendUnsubscribeAsync(ClientWebSocket socket, IReadOnlyCollection<string> symbols, CancellationToken cancellationToken)
    {
        if (symbols.Count == 0)
            return;

        var message = JsonSerializer.Serialize(new { unsubscribe = symbols });
        await SendTextAsync(socket, message, cancellationToken);
    }

    private static async Task SendTextAsync(ClientWebSocket socket, string message, CancellationToken cancellationToken)
    {
        var bytes = Encoding.UTF8.GetBytes(message);
        await socket.SendAsync(bytes, WebSocketMessageType.Text, true, cancellationToken);
    }

    private ValueTask ReleaseAnalysisLocksAsync(IReadOnlyCollection<string> symbols)
    {
        lock (_stateLock)
        {
            foreach (var symbol in symbols)
            {
                if (!_analysisLockCounts.TryGetValue(symbol, out var count))
                    continue;

                if (count <= 1)
                {
                    _analysisLockCounts.Remove(symbol);
                }
                else
                {
                    _analysisLockCounts[symbol] = count - 1;
                }
            }
        }

        SignalSubscriptionChange();
        return ValueTask.CompletedTask;
    }

    private bool HasEffectiveSubscriptions()
    {
        lock (_stateLock)
        {
            if (_analysisLockCounts.Count > 0)
                return true;

            foreach (var subscriptions in _consumerSubscriptions.Values)
            {
                if (subscriptions.Count > 0)
                    return true;
            }

            return false;
        }
    }

    private HashSet<string> GetEffectiveSubscriptions()
    {
        lock (_stateLock)
        {
            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var subscriptions in _consumerSubscriptions.Values)
            {
                result.UnionWith(subscriptions);
            }

            foreach (var symbol in _analysisLockCounts.Keys)
            {
                result.Add(symbol);
            }

            return result;
        }
    }

    private async Task WaitForSubscriptionChangeAsync(CancellationToken cancellationToken)
    {
        await _subscriptionChanges.Reader.WaitToReadAsync(cancellationToken);
    }

    private void DrainSubscriptionSignals()
    {
        while (_subscriptionChanges.Reader.TryRead(out _))
        {
        }
    }

    private void SignalSubscriptionChange()
    {
        _subscriptionChanges.Writer.TryWrite(true);
    }
}