namespace Qlarissa.WebAPI.Models.Watchlist;

public sealed class UserWatchlists
{
    public List<WatchedSecurity> PrimaryWatchlist { get; set; } = [];

    public List<WatchedSecurity> SecondaryWatchlist { get; set; } = [];

    public static UserWatchlists FromDomainEntity(IEnumerable<Domain.WatchList> domainWatchlists)
    {
        var domainWatchlistsAsList = domainWatchlists.ToList();
        return new UserWatchlists
        {
            PrimaryWatchlist = domainWatchlistsAsList[0].WatchedSecurities.Select(WatchedSecurity.FromDomainEntity).ToList(),
            SecondaryWatchlist = domainWatchlistsAsList[1].WatchedSecurities.Select(WatchedSecurity.FromDomainEntity).ToList()
        };
    }
}