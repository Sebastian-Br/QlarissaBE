using System;
using System.Collections.Generic;
using System.Text;

namespace Qlarissa.WebAPI.Tests;

public sealed class WatchlistsTests
{
    [Fact]
    public void WatchedSecurity_FromDomainEntity()
    {
        Domain.Watchlists.WatchedSecurity domainEntity = new()
        {
            SecurityId = 1,
            Name = "Some Company",
            Symbol = "SOCO",
            PreviousClosePrice = 234.5
        };

        var apiModel = Models.Watchlist.WatchedSecurity.FromDomainEntity(domainEntity);

        Assert.NotNull(apiModel);
        Assert.Equal(domainEntity.SecurityId, apiModel.SecurityId);
        Assert.Equal(domainEntity.Name, apiModel.Name);
        Assert.Equal(domainEntity.Symbol, apiModel.Symbol);
        Assert.Equal(domainEntity.PreviousClosePrice, apiModel.PreviousClosePrice);
    }
}
