namespace Qlarissa.WebAPI.Models.Watchlist;

public class WatchlistMinimal
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool IsPrimary { get; set; }

    public bool IsSecondary { get; set; }

    public bool IsPinned { get; set; }

    public int PinPosition { get; set; }

    public int TrackedChangeDurationTradingDays { get; set; }

    public static WatchlistMinimal FromDomainEntity(Domain.Watchlists.WatchlistMinimal domainWatchlist)
    {
        return new WatchlistMinimal
        {
            Id = domainWatchlist.Id,
            Name = domainWatchlist.Name,
            IsPrimary = domainWatchlist.IsPrimary,
            IsSecondary = domainWatchlist.IsSecondary,
            IsPinned = domainWatchlist.IsPinned,
            PinPosition = domainWatchlist.PinPosition,
            TrackedChangeDurationTradingDays = domainWatchlist.TrackedChangeDurationTradingDays
        };
    }

    public Domain.Watchlists.WatchlistMinimal ToDomainEntity()
    {
        return new Domain.Watchlists.WatchlistMinimal
        {
            Id = Id,
            Name = Name,
            IsPrimary = IsPrimary,
            IsSecondary = IsSecondary,
            IsPinned = IsPinned,
            PinPosition = PinPosition,
            TrackedChangeDurationTradingDays = TrackedChangeDurationTradingDays
        };
    }
}
