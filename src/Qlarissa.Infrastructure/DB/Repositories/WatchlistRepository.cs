using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZiggyCreatures.Caching.Fusion;

using Qlarissa.Application.Interfaces.Repositories;
using Qlarissa.Infrastructure.DB.Entities.Watchlists;

namespace Qlarissa.Infrastructure.DB.Repositories;

public sealed class WatchlistRepository(ILogger<SecurityRepository> logger, IFusionCache cache, ApplicationDbContext context) : IWatchlistRepository
{
    private readonly ILogger<SecurityRepository> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly IFusionCache _cache = cache ?? throw new ArgumentNullException(nameof(cache));

    private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<Domain.Watchlists.Watchlist> AddWatchlistAsync(Domain.Watchlists.Watchlist watchlist, string userId)
    {
        var dbEntity = Watchlist.FromDomainEntityWithoutCollections(watchlist, userId);
        await _context.Watchlists.AddAsync(dbEntity);
        await _context.SaveChangesAsync();

        return dbEntity.ToDomainEntityWithoutCollections();
    }

    public async Task RemoveWatchlistAsync(int watchlistId, string userId)
    {
        var affectedRows = await _context.Watchlists
            .Where(w => w.Id == watchlistId && w.UserId == userId)
            .ExecuteDeleteAsync();

        if (affectedRows == 0)
        {
            _logger.LogWarning("No watchlist found with id {WatchlistId} for user {UserId}", watchlistId, userId);
            throw new InvalidOperationException($"No watchlist found with id {watchlistId} for user {userId}");
        }
    }

    public async Task<Domain.Watchlists.Watchlist> UpdateWatchlistAsync(Domain.Watchlists.Watchlist watchlist, string userId)
    {
        var dbEntity = await _context.Watchlists
            .Where(w => w.Id == watchlist.Id && w.UserId == userId)
            .FirstAsync();

        dbEntity.Name = watchlist.Name;
        dbEntity.IsPinned = watchlist.IsPinned;
        dbEntity.TrackedChangeDurationTradingDays = watchlist.TrackedChangeDurationTradingDays;

        await _context.SaveChangesAsync();
        return dbEntity.ToDomainEntityWithoutCollections();
    }

    public async Task<IEnumerable<Domain.Watchlists.Watchlist>> GetWatchlistsAsync(string userId, CancellationToken cancellationToken)
    {
        var domainWatchlists = await _context.Watchlists
            .Where(w => w.UserId == userId)
            .Include(w => w.WatchedSecurities)
            .Select(w => new Domain.Watchlists.Watchlist()
            {
                Id = w.Id,
                Name = w.Name,
                IsPinned = w.IsPinned,
                TrackedChangeDurationTradingDays = w.TrackedChangeDurationTradingDays,
                WatchedSecurities = w.WatchedSecurities.Select(ws => new Domain.Watchlists.WatchedSecurity()
                {
                    SecurityId = ws.SecurityId,
                    Name = ws.Security.Name,
                    Symbol = ws.Security.Symbol,
                    PreviousClosePrice = ws.Security.PriceHistory
                                            .OrderByDescending(ph => ph.Id) // we can assume ascending order for price history ids of a security by default.
                                            .Skip(w.TrackedChangeDurationTradingDays - 1)
                                            .Select(ph => ph.Close)
                                            .FirstOrDefault()
                }).ToList()
            })
            .ToListAsync(cancellationToken);

        return domainWatchlists;
    }

    // might use this later if caching is desired
    private async Task<Domain.Watchlists.Watchlist> GetWatchlistNoPermissionCheck(int watchlistId, CancellationToken cancellationToken)
    {
        var domainWatchlist = await _context.Watchlists
            .Where(w => w.Id == watchlistId)
            .Include(w => w.WatchedSecurities)
            .Select(w => new Domain.Watchlists.Watchlist()
            {
                Id = w.Id,
                Name = w.Name,
                IsPinned = w.IsPinned,
                TrackedChangeDurationTradingDays = w.TrackedChangeDurationTradingDays,
                WatchedSecurities = w.WatchedSecurities.Select(ws => new Domain.Watchlists.WatchedSecurity()
                {
                    SecurityId = ws.SecurityId,
                    Name = ws.Security.Name,
                    Symbol = ws.Security.Symbol,
                    PreviousClosePrice = ws.Security.PriceHistory
                                            .OrderByDescending(ph => ph.Id) // we can assume ascending order for price history ids of a security by default.
                                            .Skip(w.TrackedChangeDurationTradingDays - 1)
                                            .Select(ph => ph.Close)
                                            .FirstOrDefault()
                }).ToList()
            })
            .FirstAsync(cancellationToken); // not finding a previously queried id is exceptional.

        return domainWatchlist;
    }

    public async Task<IEnumerable<int>> GetWatchlistsForSecurityAsync(int securityId, string userId)
    {
        var watchlistIds = await _context.Watchlists
            .Where(w => w.UserId == userId && w.WatchedSecurities.Any(ws => ws.SecurityId == securityId))
            .Select(w => w.Id)
            .ToListAsync();

        return watchlistIds;
    }

    public async Task WatchSecurityAsync(int watchlistId, int securityId, string userId)
    {
        var watchlist = await _context.Watchlists
            .Where(w => w.Id == watchlistId && w.UserId == userId)
            .Include(w => w.WatchedSecurities)
            .FirstAsync();

        watchlist.WatchedSecurities.Add(new WatchedSecurity()
        {
            WatchlistId = watchlistId,
            SecurityId = securityId
        });

        await _context.SaveChangesAsync();
    }

    public async Task UnwatchSecurityAsync(int watchlistId, int securityId, string userId)
    {
        var watchlist = await _context.Watchlists
            .Where(w => w.Id == watchlistId && w.UserId == userId)
            .Include(w => w.WatchedSecurities)
            .FirstAsync();

        var watchedSecurity = watchlist.WatchedSecurities.First(ws => ws.SecurityId == securityId);

        _context.WatchedSecurities.Remove(watchedSecurity);
        await _context.SaveChangesAsync();
    }
}
