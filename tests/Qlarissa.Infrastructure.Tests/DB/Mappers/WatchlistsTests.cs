
namespace Qlarissa.Infrastructure.Tests.DB.Mappers;

public sealed class WatchlistsTests
{

    [Fact]
    public void Watchlist_FromDomainEntityWithoutCollections()
    {
        var domainWatchlist = new Domain.Watchlists.Watchlist
        {
            Id = 1,
            Name = "My Watchlist",
            IsPinned = true,
            PinPosition = 1,
            TrackedChangeDurationTradingDays = 5,
            WatchedSecurities = new List<Domain.Watchlists.WatchedSecurity>
            {
                new() {
                    SecurityId = 101,
                    Name = "Company A",
                    Symbol = "CMPA",
                    PreviousClosePrice = 100.5
                },
                new() {
                    SecurityId = 102,
                    Name = "Company B",
                    Symbol = "CMPB",
                    PreviousClosePrice = 200.75
                }
            }
        };

        string userId = "user123";
        var entityWatchlist = Infrastructure.DB.Entities.Watchlists.Watchlist.FromDomainEntityWithoutCollections(domainWatchlist, userId);
        
        Assert.Equal(domainWatchlist.Id, entityWatchlist.Id);
        Assert.Equal(userId, entityWatchlist.UserId);
        Assert.Equal(domainWatchlist.Name, entityWatchlist.Name);
        Assert.Equal(domainWatchlist.IsPinned, entityWatchlist.IsPinned);
        Assert.Equal(domainWatchlist.PinPosition, entityWatchlist.PinPosition);
        Assert.Equal(domainWatchlist.TrackedChangeDurationTradingDays, entityWatchlist.TrackedChangeDurationTradingDays);
        Assert.Empty(entityWatchlist.WatchedSecurities); // WatchedSecurities should not be mapped in this method
    }

    [Fact]
    public void Watchlist_ToDomainEntityWithoutCollections()
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
        var domainWatchlist = entityWatchlist.ToDomainEntity();
        Assert.Equal(entityWatchlist.Id, domainWatchlist.Id);
        Assert.Equal(entityWatchlist.Name, domainWatchlist.Name);
        Assert.Equal(entityWatchlist.IsPinned, domainWatchlist.IsPinned);
        Assert.Equal(entityWatchlist.PinPosition, domainWatchlist.PinPosition);
        Assert.Equal(entityWatchlist.TrackedChangeDurationTradingDays, domainWatchlist.TrackedChangeDurationTradingDays);
        Assert.Empty(domainWatchlist.WatchedSecurities); // WatchedSecurities should not be mapped in this method
    }
}
