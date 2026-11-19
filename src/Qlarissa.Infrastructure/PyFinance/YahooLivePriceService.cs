using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Qlarissa.Application.Interfaces.ExternalAPI;
using Qlarissa.Application.Interfaces.Repositories;
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
    private readonly IServiceScopeFactory _scopeFactory;

    private readonly object _stateLock = new();
    private readonly HashSet<string> _subscribedSymbols = new(StringComparer.OrdinalIgnoreCase);

    // symbol -> latest received price
    private readonly Dictionary<string, LivePrice> _latestPrices = new(StringComparer.OrdinalIgnoreCase);

    private readonly Channel<bool> _subscriptionChanges = Channel.CreateBounded<bool>(
        new BoundedChannelOptions(1)
        {
            SingleWriter = false,
            SingleReader = true,
            FullMode = BoundedChannelFullMode.DropWrite
        });

    public YahooLivePriceService(
        ILogger<YahooLivePriceService> logger,
        IOptions<LivePriceOptions> options,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        ArgumentNullException.ThrowIfNull(options);
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));

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

    public Task SubscribeAsync(IEnumerable<string> symbols, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(symbols);
        cancellationToken.ThrowIfCancellationRequested();

        var normalizedSymbols = symbols
            .Where(symbol => !string.IsNullOrWhiteSpace(symbol))
            .Select(symbol => symbol.Trim().ToUpperInvariant())
            .ToArray();

        var hasNewSymbols = false;
        lock (_stateLock)
        {
            foreach (var symbol in normalizedSymbols)
            {
                hasNewSymbols |= _subscribedSymbols.Add(symbol);
            }
        }

        if (hasNewSymbols)
        {
            _subscriptionChanges.Writer.TryWrite(true);
        }

        return Task.CompletedTask;
    }

    public IReadOnlyList<LivePrice> GetLatestPrices()
    {
        lock (_stateLock)
        {
            return _latestPrices.Values.ToArray();
        }
    }

    public IReadOnlyList<LivePrice> GetPrices(IEnumerable<string> symbols)
    {
        ArgumentNullException.ThrowIfNull(symbols);
        var requestedSymbols = symbols.ToHashSet(StringComparer.OrdinalIgnoreCase);

        lock (_stateLock)
        {
            return _latestPrices
                .Where(price => requestedSymbols.Contains(price.Key))
                .Select(price => price.Value)
                .ToArray();
        }
    }

    // BackgroundService
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Yahoo live-price service started.");

        try
        {
            string[] symbols;
            await using (var scope = _scopeFactory.CreateAsyncScope())
            {
                var securityRepository = scope.ServiceProvider.GetRequiredService<ISecurityRepository>();
                symbols = (await securityRepository.GetAllSecuritySymbolsAsync()).ToArray();
            }

            await SubscribeAsync(symbols, stoppingToken);
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
            try
            {
                await RunYahooConnectionAsync(stoppingToken);
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
                    await Task.Delay(reconnectDelay, stoppingToken);
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

        var currentSubscriptions = GetSubscribedSymbols();
        await SendSubscribeAsync(socket, currentSubscriptions, stoppingToken);

        // Exactly one receive operation is active for this socket.
        var receiveTask = ReceiveLoopAsync(socket, stoppingToken);
        var heartbeatTask = Task.Delay(TimeSpan.FromSeconds(YahooHeartbeatSeconds), stoppingToken);
        var subscriptionChangeTask = WaitForSubscriptionChangeAsync(stoppingToken);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var completedTask = await Task.WhenAny(receiveTask, heartbeatTask, subscriptionChangeTask);

                if (completedTask == receiveTask)
                {
                    await receiveTask;
                    return;
                }

                if (completedTask == subscriptionChangeTask)
                {
                    DrainSubscriptionChanges();
                    var desiredSubscriptions = GetSubscribedSymbols();
                    var addedSymbols = desiredSubscriptions
                        .Except(currentSubscriptions, StringComparer.OrdinalIgnoreCase)
                        .ToArray();

                    await SendSubscribeAsync(socket, addedSymbols, stoppingToken);
                    currentSubscriptions = desiredSubscriptions;
                    subscriptionChangeTask = WaitForSubscriptionChangeAsync(stoppingToken);
                    continue;
                }

                currentSubscriptions = GetSubscribedSymbols();
                await SendSubscribeAsync(socket, currentSubscriptions, stoppingToken);
                heartbeatTask = Task.Delay(TimeSpan.FromSeconds(YahooHeartbeatSeconds), stoppingToken);
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

            /*_logger.LogInformation(
                "Received live price for {Symbol}: {Price} at {Timestamp} (day high {DayHigh}, day low {DayLow}).",
                livePrice.Symbol,
                livePrice.Price,
                livePrice.Timestamp,
                livePrice.DayHigh,
                livePrice.DayLow);*/
        }
        catch (Exception ex)
        {
            // A malformed individual Yahoo message should not terminate the entire WebSocket connection.
            _logger.LogWarning(ex, "Failed to decode a Yahoo Finance WebSocket message.");
        }
    }

    private HashSet<string> GetSubscribedSymbols()
    {
        lock (_stateLock)
        {
            return new HashSet<string>(_subscribedSymbols, StringComparer.OrdinalIgnoreCase);
        }
    }

    private async Task WaitForSubscriptionChangeAsync(CancellationToken cancellationToken)
    {
        await _subscriptionChanges.Reader.WaitToReadAsync(cancellationToken);
    }

    private void DrainSubscriptionChanges()
    {
        while (_subscriptionChanges.Reader.TryRead(out _))
        {
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

    private static async Task SendTextAsync(ClientWebSocket socket, string message, CancellationToken cancellationToken)
    {
        var bytes = Encoding.UTF8.GetBytes(message);
        await socket.SendAsync(bytes, WebSocketMessageType.Text, true, cancellationToken);
    }
}