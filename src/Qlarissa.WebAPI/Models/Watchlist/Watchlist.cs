namespace Qlarissa.WebAPI.Models.Watchlist;

public sealed class Watchlist : WatchlistMinimal
{
    public List<WatchedSecurity> WatchedSecurities { get; set; } = [];

    public static Watchlist FromDomainEntity(Domain.Watchlists.Watchlist domainWatchlist)
    {
        return new Watchlist
        {
            Id = domainWatchlist.Id,
            Name = domainWatchlist.Name,
            IsPinned = domainWatchlist.IsPinned,
            PinPosition = domainWatchlist.PinPosition,
            TrackedChangeDurationTradingDays = domainWatchlist.TrackedChangeDurationTradingDays,
            WatchedSecurities = domainWatchlist.WatchedSecurities.Select(WatchedSecurity.FromDomainEntity).ToList()
        };
    }
}
