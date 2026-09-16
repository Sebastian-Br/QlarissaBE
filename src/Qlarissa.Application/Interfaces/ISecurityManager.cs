using Qlarissa.Domain;
using Qlarissa.Domain.Securities;
using Qlarissa.Domain.Securities.Base;

namespace Qlarissa.Application.Interfaces;

public interface ISecurityManager
{
    public Task<PubliclyTradedSecurityBase?> GetSecurityAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a security from an external API to the database. The currency of that security must already exist.
    /// </summary>
    /// <param name="securityTickerSymbol">The ticker symbol, e.g. "MSFT".</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Returns an Ok() result on success where the content is the added ticker symbol.</returns>
    public Task<FluentResults.Result<string>> AddSecurityAsync(string securityTickerSymbol, CancellationToken cancellationToken);

    /// <summary>
    /// Attempts to update the security from the external API.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Result.Ok if the security was updated successfully.</returns>
    public Task<FluentResults.Result> UpdateSecurityAsync(int id, CancellationToken cancellationToken);

    public Task<bool> SecurityExistsAsync(string securityTickerSymbol);
    public Task<IEnumerable<SearchResult>> SearchSecuritiesInternallyAsync(string userQuery, CancellationToken cancellationToken);
    public Task<IEnumerable<SearchResult>> SearchSecuritiesExternallyAsync(string userQuery, CancellationToken cancellationToken);

    public Task<WatchedSecurityMinimal> IsSecurityWatchedAsync(int securityId, string userId);

    /// <summary>
    /// Adds a security to the user's watchlist. If isPrimaryWatchlist is true, it will be added to the primary watchlist; otherwise, it will be added to the secondary watchlist.
    /// </summary>
    /// <param name="securityId"></param>
    /// <param name="userId"></param>
    /// <param name="isPrimaryWatchlist"></param>
    /// <returns>Result.Ok() on success.</returns>
    public Task<FluentResults.Result> WatchSecurityAsync(int securityId, string userId, bool isPrimaryWatchlist);

    /// <summary>
    /// Removes a security from the user's watchlists.
    /// A security can only be in either the primary or secondary watchlist; specifying which watchlist to remove it from is not required.
    /// </summary>
    /// <param name="securityId"></param>
    /// <param name="userId"></param>
    /// <returns>Result.Ok() on success.</returns>
    public Task<FluentResults.Result> UnwatchSecurityAsync(int securityId, string userId);

    /// <summary>
    /// Retrieves the primary and secondary watchlists for a user.
    /// </summary>
    /// <param name="userId">The user id.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    /// The primary watchlist is the first member, the secondary watchlist is the second member in the collection.
    /// If a watchlist is empty, it will still be returned, but contain an empty WatchedSecurities collection.
    /// </returns>
    public Task<IEnumerable<WatchList>> GetWatchlistsAsync(string userId, CancellationToken cancellationToken);
}