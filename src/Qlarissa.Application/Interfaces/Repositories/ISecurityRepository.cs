using Qlarissa.Domain;
using Qlarissa.Domain.Securities;
using Qlarissa.Domain.Securities.Base;

namespace Qlarissa.Application.Interfaces.Repositories;

public interface ISecurityRepository
{
    /// <summary>
    /// Gets a security by its ID. Returns null if the security does not exist.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PubliclyTradedSecurityBase?> GetSecurityAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a security with its currency by ID, but does not load navigation collections. 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Returns null if the security does not exist.</returns>
    Task<PubliclyTradedSecurityBase?> GetSecurityBasicAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the date of the last data point in the price history for a security.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Returns null if the security does not exist.</returns>
    Task<SecuritySymbolAndLastDataPointDate?> GetSecuritySymbolAndPriceHistoryLastDataPointDateAsync(int id);

    /// <summary>
    /// Adds a security to the database. The currency of that security must already exist.
    /// </summary>
    /// <param name="security"></param>
    /// <returns></returns>
    Task AddSecurityAsync(PubliclyTradedSecurityBase security, CancellationToken cancellationToken);

    Task<bool> SecurityExistsAsync(string tickerSymbol);

    /// <summary>
    /// Searches for securities based on the user query. The search will be performed on the name and symbol of the security.
    /// </summary>
    /// <param name="userQuery">E.g. "Micros"</param>
    /// <param name="cancellationToken"></param>
    /// <returns>SearchResults, containing e.g. { Name = "Microsoft Corporation", ... }</returns>
    Task<IEnumerable<SearchResult>> SearchSecuritiesAsync(string userQuery, CancellationToken cancellationToken);

    /// <summary>
    /// Updates the properties of a security.
    /// Must automatically detect split events and update existing price history accordingly.
    /// </summary>
    /// <param name="security"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<FluentResults.Result> UpdateSecurityAsync(PubliclyTradedSecurityBase security, CancellationToken cancellationToken);

    Task<FluentResults.Result> WatchSecurityAsync(int securityId, string userId, bool isPrimaryWatchlist);

    Task<FluentResults.Result> UnwatchSecurityAsync(int securityId, string userId);

    /// <summary>
    /// Checks if a security is watched by a user and if it is on the user's primary watchlist.
    /// </summary>
    /// <param name="securityId"></param>
    /// <param name="userId"></param>
    /// <returns>Never returns null. If the security does not exist, it will return IsWatched = false.</returns>
    Task<WatchedSecurityMinimal> IsSecurityWatchedAsync(int securityId, string userId);

    Task<IEnumerable<WatchList>> GetWatchlistsAsync(string userId, CancellationToken cancellationToken);

    public record SecuritySymbolAndLastDataPointDate(string Symbol, DateOnly PriceHistoryLastDataPointDate);
}