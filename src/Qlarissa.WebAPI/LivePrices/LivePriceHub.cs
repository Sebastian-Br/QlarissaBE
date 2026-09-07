using Microsoft.AspNetCore.SignalR;
using Qlarissa.Application.Interfaces.ExternalAPI;

namespace Qlarissa.WebAPI.LivePrices;

public sealed class LivePriceHub(ILivePriceService livePriceService) : Hub
{
    private readonly ILivePriceService _livePriceService = livePriceService;

    public async Task Subscribe(IEnumerable<string> symbols)
    {
        var normalizedSymbols = NormalizeSymbols(symbols);

        if (normalizedSymbols.Count == 0)
            return;

        await _livePriceService.SubscribeAsync(
            Context.ConnectionId,
            normalizedSymbols,
            CancellationToken.None);

        // Immediately provide the latest cached prices, if available.
        var prices = _livePriceService.GetPrices(normalizedSymbols);

        if (prices.Count > 0)
        {
            await Clients.Caller.SendAsync(
                "PriceSnapshot",
                prices);
        }
    }

    public async Task Unsubscribe(IEnumerable<string> symbols)
    {
        var normalizedSymbols = NormalizeSymbols(symbols);

        if (normalizedSymbols.Count == 0)
            return;

        await _livePriceService.UnsubscribeAsync(
            Context.ConnectionId,
            normalizedSymbols,
            CancellationToken.None);
    }

    public override async Task OnDisconnectedAsync(
        Exception? exception)
    {
        await _livePriceService.RemoveConsumerAsync(
            Context.ConnectionId,
            CancellationToken.None);

        await base.OnDisconnectedAsync(exception);
    }

    private static IReadOnlyCollection<string> NormalizeSymbols(
        IEnumerable<string> symbols)
    {
        return symbols
            .Where(symbol => !string.IsNullOrWhiteSpace(symbol))
            .Select(symbol => symbol.Trim().ToUpperInvariant())
            .Distinct(StringComparer.Ordinal)
            .ToArray();
    }
}