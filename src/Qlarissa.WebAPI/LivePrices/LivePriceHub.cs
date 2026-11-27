using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Qlarissa.Application.Interfaces.ExternalAPI;

namespace Qlarissa.WebAPI.LivePrices;

[Authorize]
public sealed class LivePriceHub(ILivePriceService livePriceService) : Hub
{
    private readonly ILivePriceService _livePriceService = livePriceService ?? throw new ArgumentNullException(nameof(livePriceService));

    public async Task<IReadOnlyList<Domain.Securities.MarketData.LivePrice>> Subscribe(IEnumerable<string> symbols)
    {
        var normalizedSymbols = NormalizeSymbols(symbols);

        foreach (var symbol in normalizedSymbols)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName(symbol));
        }

        return _livePriceService.GetPrices(normalizedSymbols);
    }

    public async Task Unsubscribe(IEnumerable<string> symbols)
    {
        var normalizedSymbols = NormalizeSymbols(symbols);

        foreach (var symbol in normalizedSymbols)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetGroupName(symbol));
        }
    }

    internal static string GetGroupName(string symbol) => $"live-price:{symbol}";

    private static IReadOnlyCollection<string> NormalizeSymbols(IEnumerable<string> symbols)
    {
        ArgumentNullException.ThrowIfNull(symbols);

        return symbols
            .Where(symbol => !string.IsNullOrWhiteSpace(symbol))
            .Select(symbol => symbol.Trim().ToUpperInvariant())
            .Distinct(StringComparer.Ordinal)
            .ToArray();
    }
}