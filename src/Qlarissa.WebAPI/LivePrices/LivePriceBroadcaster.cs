

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Qlarissa.Application.Interfaces.ExternalAPI;
using Qlarissa.Domain.Entities.Securities.MarketData;
using Qlarissa.Infrastructure.PyFinance.Options;

namespace Qlarissa.WebAPI.LivePrices;

public sealed class LivePriceBroadcaster(
    ILivePriceService livePriceService,
    IHubContext<LivePriceHub> hubContext,
    IOptions<LivePriceOptions> options,
    ILogger<LivePriceBroadcaster> logger)
    : BackgroundService
{
    private readonly ILivePriceService _livePriceService = livePriceService;
    private readonly IHubContext<LivePriceHub> _hubContext = hubContext;
    private readonly LivePriceOptions _options = options.Value;
    private readonly ILogger<LivePriceBroadcaster> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Live price broadcasting service started.");
        using var timer = new PeriodicTimer(_options.BatchInterval);

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await BroadcastLatestPricesAsync(stoppingToken);
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            // Normal application shutdown.
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Live price broadcaster stopped unexpectedly.");
        }
    }

    private async Task BroadcastLatestPricesAsync(CancellationToken cancellationToken)
    {
        var consumerSubscriptions = _livePriceService.GetConsumerSubscriptions();

        if (consumerSubscriptions.Count == 0)
            return;

        var sendTasks = new List<Task>();

        foreach (var consumer in consumerSubscriptions)
        {
            var connectionId = consumer.Key;
            var symbols = consumer.Value;

            if (symbols.Count == 0)
                continue;

            var prices = _livePriceService.GetPrices(symbols);

            if (prices.Count == 0)
                continue;

            sendTasks.Add(
                SendToClientAsync(
                    connectionId,
                    prices,
                    cancellationToken));
        }

        await Task.WhenAll(sendTasks);
    }

    private async Task SendToClientAsync(string connectionId, IReadOnlyList<LivePrice> prices, CancellationToken cancellationToken)
    {
        try
        {
            await _hubContext.Clients
                .Client(connectionId)
                .SendAsync(
                    "PriceBatch",
                    prices,
                    cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            // The client may have disconnected between taking the subscription snapshot and sending the message.
            // OnDisconnectedAsync is responsible for removing the consumer from the live-price service.
            _logger.LogDebug(ex, "Could not send live-price update to SignalR connection {ConnectionId}.", connectionId);
        }
    }
}