using Qlarissa.Domain.Securities.MarketData;

namespace Qlarissa.Application.Interfaces.ExternalAPI;

public interface ILivePriceService
{
    /// <summary>
    /// Ensures the application is subscribed to the specified symbols. Repeated symbols are ignored.
    /// </summary>
    Task SubscribeAsync(IEnumerable<string> symbols, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the latest known prices. Symbols for which no price has been received yet are omitted.
    /// </summary>
    IReadOnlyList<LivePrice> GetLatestPrices();

    /// <summary>
    /// Returns the latest known prices for the requested symbols.
    /// </summary>
    IReadOnlyList<LivePrice> GetPrices(IEnumerable<string> symbols);
}