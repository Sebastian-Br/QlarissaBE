using Qlarissa.Application.Interfaces;
using Qlarissa.Application.Interfaces.Repositories;
using Qlarissa.Domain.Watchlists;
using System;
using System.Collections.Generic;
using System.Text;

namespace Qlarissa.Application;

public sealed class WatchlistManager(IWatchlistRepository watchlistRepository) : IWatchlistManager
{
    private readonly IWatchlistRepository _watchlistRepository = watchlistRepository ?? throw new ArgumentNullException(nameof(watchlistRepository));

    public Task<Watchlist> AddWatchlistAsync(Watchlist watchlist, string userId)
    {
        return _watchlistRepository.AddWatchlistAsync(watchlist, userId);
    }

    public Task<IEnumerable<Watchlist>> GetAllWatchlistsAsync(string userId, CancellationToken cancellationToken)
    {
        return _watchlistRepository.GetWatchlistsAsync(userId, cancellationToken);
    }

    public Task<IEnumerable<int>> GetWatchlistsForSecurityAsync(int securityId, string userId)
    {
        return _watchlistRepository.GetWatchlistsForSecurityAsync(securityId, userId);
    }

    public Task RemoveWatchlistAsync(int watchlistId, string userId)
    {
        return _watchlistRepository.RemoveWatchlistAsync(watchlistId, userId);
    }

    public Task UnwatchSecurityAsync(int watchlistId, int securityId, string userId)
    {
        return _watchlistRepository.UnwatchSecurityAsync(watchlistId, securityId, userId);
    }

    public Task UpdateWatchlistAsync(Watchlist watchlist, string userId)
    {
        return _watchlistRepository.UpdateWatchlistAsync(watchlist, userId);
    }

    public Task WatchSecurityAsync(int watchlistId, int securityId, string userId)
    {
        return _watchlistRepository.WatchSecurityAsync(watchlistId, securityId, userId);
    }
}
