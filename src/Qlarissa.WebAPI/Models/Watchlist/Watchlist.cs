namespace Qlarissa.WebAPI.Models.Watchlist;

public class Watchlist
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool IsPinned { get; set; }

    public int PinPosition { get; set; }

    public int TrackedChangeDurationTradingDays { get; set; }

    public List<WatchedSecurity> WatchedSecurities { get; set; } = [];

    public static Watchlist FromDomainEntityWithoutCollections(Domain.Watchlists.Watchlist domainWatchlist)
    {
        return new Watchlist
        {
            Id = domainWatchlist.Id,
            Name = domainWatchlist.Name,
            IsPinned = domainWatchlist.IsPinned,
            PinPosition = domainWatchlist.PinPosition,
            TrackedChangeDurationTradingDays = domainWatchlist.TrackedChangeDurationTradingDays
        };
    }

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

    public Domain.Watchlists.Watchlist ToDomainEntityWithoutCollections()
    {
        return new Domain.Watchlists.Watchlist
        {
            Id = Id,
            Name = Name,
            IsPinned = IsPinned,
            PinPosition = PinPosition,
            TrackedChangeDurationTradingDays = TrackedChangeDurationTradingDays
            // No need to map WatchedSecurities - these are added after the watchlist has been created.
        };
    }
}
