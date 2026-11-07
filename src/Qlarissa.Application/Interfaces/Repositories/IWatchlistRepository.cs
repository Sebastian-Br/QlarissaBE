using Qlarissa.Domain.Watchlists;

namespace Qlarissa.Application.Interfaces.Repositories;

public interface IWatchlistRepository
{
    Task<WatchlistMinimal> AddWatchlistAsync(WatchlistMinimal watchlist, string userId);

    Task RemoveWatchlistAsync(int watchlistId, string userId);

    Task<WatchlistMinimal> UpdateWatchlistAsync(WatchlistMinimal watchlist, string userId);

    Task WatchSecurityAsync(int watchlistId,int securityId, string userId);

    Task UnwatchSecurityAsync(int watchlistId, int securityId, string userId);

    /// <summary>
    /// Checks which watchlists a security is on for a given user.
    /// </summary>
    /// <param name="securityId"></param>
    /// <param name="userId"></param>
    /// <returns>Ids of the watchlists the security is on, or [] if it is unwatched.</returns>
    Task<IEnumerable<int>> GetWatchlistsForSecurityAsync(int securityId, string userId);

    Task<IEnumerable<WatchlistMinimal>> GetWatchlistsMinimalAsync(string userId);

    Task<IEnumerable<Watchlist>> GetPinnedWatchlistsAsync(string userId, CancellationToken cancellationToken);

    Task<IEnumerable<Watchlist>> GetWatchlistsAsync(string userId, CancellationToken cancellationToken);
}
