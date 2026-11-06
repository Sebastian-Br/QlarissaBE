using System;
using System.Collections.Generic;
using System.Text;

namespace Qlarissa.Infrastructure.Tests.DB.Mappers;

public sealed class WatchlistsTests
{

    [Fact]
    public void Watchlist_FromDomainEntity()
    {
        var domainWatchlist = new Domain.Watchlists.Watchlist
        {
            Id = 1,
            Name = "My Watchlist",
            IsPinned = true,
            TrackedChangeDurationTradingDays = 5
        };

        string userId = "user123";
        var entityWatchlist = Infrastructure.DB.Entities.Watchlists.Watchlist.FromDomainEntityWithoutCollections(domainWatchlist, userId);
        
        Assert.Equal(domainWatchlist.Id, entityWatchlist.Id);
        Assert.Equal(userId, entityWatchlist.UserId);
        Assert.Equal(domainWatchlist.Name, entityWatchlist.Name);
        Assert.Equal(domainWatchlist.IsPinned, entityWatchlist.IsPinned);
        Assert.Equal(domainWatchlist.TrackedChangeDurationTradingDays, entityWatchlist.TrackedChangeDurationTradingDays);
    }
}
