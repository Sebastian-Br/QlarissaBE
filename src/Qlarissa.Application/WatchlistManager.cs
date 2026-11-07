using Qlarissa.Application.Interfaces;
using Qlarissa.Application.Interfaces.Repositories;
using Qlarissa.Domain.Watchlists;

namespace Qlarissa.Application;

public sealed class WatchlistManager(IWatchlistRepository watchlistRepository) : IWatchlistManager
{
    private readonly IWatchlistRepository _watchlistRepository = watchlistRepository ?? throw new ArgumentNullException(nameof(watchlistRepository));

    public Task<WatchlistMinimal> AddWatchlistAsync(WatchlistMinimal watchlist, string userId)
        => _watchlistRepository.AddWatchlistAsync(watchlist, userId);

    public Task<IEnumerable<Watchlist>> GetAllWatchlistsAsync(string userId, CancellationToken cancellationToken)
        => _watchlistRepository.GetWatchlistsAsync(userId, cancellationToken);

    public Task<IEnumerable<Watchlist>> GetPinnedWatchlistsAsync(string userId, CancellationToken cancellationToken)
        => _watchlistRepository.GetPinnedWatchlistsAsync(userId, cancellationToken);

    public Task<IEnumerable<int>> GetWatchlistsForSecurityAsync(int securityId, string userId)
        => _watchlistRepository.GetWatchlistsForSecurityAsync(securityId, userId);
    

    public Task<IEnumerable<WatchlistMinimal>> GetWatchlistsMinimalAsync(string userId)
        => _watchlistRepository.GetWatchlistsMinimalAsync(userId);

    public Task RemoveWatchlistAsync(int watchlistId, string userId)
        => _watchlistRepository.RemoveWatchlistAsync(watchlistId, userId);

    public Task UnwatchSecurityAsync(int watchlistId, int securityId, string userId)
        => _watchlistRepository.UnwatchSecurityAsync(watchlistId, securityId, userId);

    public Task<WatchlistMinimal> UpdateWatchlistAsync(WatchlistMinimal watchlist, string userId)
        => _watchlistRepository.UpdateWatchlistAsync(watchlist, userId);

    public Task WatchSecurityAsync(int watchlistId, int securityId, string userId)
        => _watchlistRepository.WatchSecurityAsync(watchlistId, securityId, userId);
}
