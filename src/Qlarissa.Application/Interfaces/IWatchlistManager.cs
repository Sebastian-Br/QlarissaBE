using Qlarissa.Domain.Watchlists;

namespace Qlarissa.Application.Interfaces;

public interface IWatchlistManager
{
    /// <summary>
    /// Adds a watchlist and returns the newly created watchlist.
    /// </summary>
    /// <param name="watchlist"></param>
    /// <param name="userId"></param>
    /// <returns>The newly created watchlist with its Id property filled.</returns>
    Task<Watchlist> AddWatchlistAsync(Watchlist watchlist, string userId);

    Task RemoveWatchlistAsync(int watchlistId, string userId);

    // Only updates the Name, Pinned status, and TrackedChangeDurationTradingDays, NOT the WatchedSecurities.
    Task UpdateWatchlistAsync(Watchlist watchlist, string userId);

    Task WatchSecurityAsync(int watchlistId, int securityId, string userId);

    Task UnwatchSecurityAsync(int watchlistId, int securityId, string userId);

    /// <summary>
    /// Checks which watchlists a security is on for a given user.
    /// </summary>
    /// <param name="securityId"></param>
    /// <param name="userId"></param>
    /// <returns>Ids of the watchlists the security is on, or [] if it is unwatched.</returns>
    Task<IEnumerable<int>> GetWatchlistsForSecurityAsync(int securityId, string userId);

    Task<IEnumerable<Watchlist>> GetAllWatchlistsAsync(string userId, CancellationToken cancellationToken);
}
