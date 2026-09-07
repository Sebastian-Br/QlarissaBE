using Qlarissa.Domain.Entities.Securities;

namespace Qlarissa.Application.Interfaces.ExternalAPI;

public interface IMarketDataClient
{
    /// <summary>
    /// Searches for a security via an external API.
    /// </summary>
    /// <param name="userQuery"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>SearchResult collection</returns>
    public Task<IEnumerable<SearchResult>> SearchSecuritiesAsync(string userQuery, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the security with its price, dividend, and split history for the given ticker symbol.
    /// Drops the last history entry if its Close price is 0, since that indicates the market is still open and the price is not final.
    /// Live price data must be used to reconstruct the current price of the security, for which a manager is more appropriate instead of persisting that data and hitting the DB each time.
    /// </summary>
    /// <param name="tickerSymbol"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<Domain.Entities.Securities.Base.PubliclyTradedSecurityBase> GetSecurityWithHistoryAsync(string tickerSymbol, CancellationToken cancellationToken);
}