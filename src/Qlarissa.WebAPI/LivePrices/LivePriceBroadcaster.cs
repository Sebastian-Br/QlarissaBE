

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Qlarissa.Application.Interfaces.ExternalAPI;
using Qlarissa.Infrastructure.PyFinance.Options;

namespace Qlarissa.WebAPI.LivePrices;

public sealed class LivePriceBroadcaster(
    ILivePriceService livePriceService,
    IHubContext<LivePriceHub> hubContext,
    IOptions<LivePriceOptions> options,
    ILogger<LivePriceBroadcaster> logger)
    : BackgroundService
{
    private readonly ILivePriceService _livePriceService = livePriceService ?? throw new ArgumentNullException(nameof(livePriceService));
    private readonly IHubContext<LivePriceHub> _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
    private readonly LivePriceOptions _options = options.Value ?? throw new ArgumentNullException(nameof(options));
    private readonly ILogger<LivePriceBroadcaster> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

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
        var prices = _livePriceService.GetLatestPrices();

        if (prices.Count == 0)
            return;

        var sendTasks = prices.Select(price =>
            _hubContext.Clients
                .Group(LivePriceHub.GetGroupName(price.Symbol))
                .SendAsync("PriceBatch", new[] { price }, cancellationToken));

        try
        {
            await Task.WhenAll(sendTasks);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "Could not broadcast live-price updates to subscribed SignalR clients.");
        }
    }
}