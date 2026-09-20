namespace Qlarissa.WebAPI.Models.Watchlist;

public sealed class UserWatchlists
{
    public List<WatchedSecurity> PrimaryWatchlist { get; set; } = [];

    public List<WatchedSecurity> SecondaryWatchlist { get; set; } = [];

    public static UserWatchlists FromDomainEntity(IEnumerable<Domain.WatchList> domainWatchlists)
    {
        return new UserWatchlists
        {
            PrimaryWatchlist = domainWatchlists.FirstOrDefault(ws => ws.IsPrimary)?.WatchedSecurities.Select(WatchedSecurity.FromDomainEntity).ToList() ?? [],
            SecondaryWatchlist = domainWatchlists.FirstOrDefault(ws => !ws.IsPrimary)?.WatchedSecurities.Select(WatchedSecurity.FromDomainEntity).ToList() ?? []
        };
    }
}