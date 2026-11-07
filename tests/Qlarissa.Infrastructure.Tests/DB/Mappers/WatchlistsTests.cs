
namespace Qlarissa.Infrastructure.Tests.DB.Mappers;

public sealed class WatchlistsTests
{

    [Fact]
    public void Watchlist_FromDomainEntityWithoutCollections()
    {
        var domainWatchlist = new Domain.Watchlists.WatchlistMinimal
        {
            Id = 1,
            Name = "My Watchlist",
            IsPinned = true,
            PinPosition = 1,
            TrackedChangeDurationTradingDays = 5,
        };

        string userId = "user123";
        var entityWatchlist = Infrastructure.DB.Entities.Watchlists.Watchlist.FromDomainEntityWithoutCollections(domainWatchlist, userId);
        
        Assert.Equal(domainWatchlist.Id, entityWatchlist.Id);
        Assert.Equal(userId, entityWatchlist.UserId);
        Assert.Equal(domainWatchlist.Name, entityWatchlist.Name);
        Assert.Equal(domainWatchlist.IsPrimary, entityWatchlist.IsPrimary);
        Assert.Equal(domainWatchlist.IsSecondary, entityWatchlist.IsSecondary);
        Assert.Equal(domainWatchlist.IsPinned, entityWatchlist.IsPinned);
        Assert.Equal(domainWatchlist.PinPosition, entityWatchlist.PinPosition);
        Assert.Equal(domainWatchlist.TrackedChangeDurationTradingDays, entityWatchlist.TrackedChangeDurationTradingDays);
        Assert.Empty(entityWatchlist.WatchedSecurities); // WatchedSecurities should not be mapped in this method
    }

    [Fact]
    public void Watchlist_ToDomainEntity()
    {
        var entityWatchlist = new Infrastructure.DB.Entities.Watchlists.Watchlist
        {
            Id = 1,
            UserId = "user123",
            Name = "My Watchlist",
            IsPinned = true,
            PinPosition = 1,
            TrackedChangeDurationTradingDays = 5,
            WatchedSecurities = new List<Infrastructure.DB.Entities.Watchlists.WatchedSecurity>
            {
                new() {
                    WatchlistId = 1,
                    SecurityId = 101,
                },
                new() {
                    WatchlistId = 1,
                    SecurityId = 102,
                }
            }
        };
        var domainWatchlistMinimal = entityWatchlist.ToDomainEntity();
        Assert.Equal(entityWatchlist.Id, domainWatchlistMinimal.Id);
        Assert.Equal(entityWatchlist.Name, domainWatchlistMinimal.Name);
        Assert.Equal(entityWatchlist.IsPrimary, domainWatchlistMinimal.IsPrimary);
        Assert.Equal(entityWatchlist.IsSecondary, domainWatchlistMinimal.IsSecondary);
        Assert.Equal(entityWatchlist.IsPinned, domainWatchlistMinimal.IsPinned);
        Assert.Equal(entityWatchlist.PinPosition, domainWatchlistMinimal.PinPosition);
        Assert.Equal(entityWatchlist.TrackedChangeDurationTradingDays, domainWatchlistMinimal.TrackedChangeDurationTradingDays);
    }
}
