namespace Qlarissa.WebAPI.Models.Watchlist;

public class WatchedSecurityMinimal
{
    public bool IsWatched { get; set; }
    public bool IsOnPrimaryWatchlist { get; set; }
    public static WatchedSecurityMinimal FromDomainEntity(Domain.WatchedSecurityMinimal domainEntity) => new()
    {
        IsWatched = domainEntity.IsWatched,
        IsOnPrimaryWatchlist = domainEntity.IsOnPrimaryWatchlist
    };
}