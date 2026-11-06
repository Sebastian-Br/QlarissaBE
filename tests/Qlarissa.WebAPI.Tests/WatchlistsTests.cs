
namespace Qlarissa.WebAPI.Tests;

public sealed class WatchlistsTests
{
    [Fact]
    public void Watchlist_FromDomainEntityWithoutCollections()
    {
        Domain.Watchlists.Watchlist domainWatchlist = new()
        {
            Id = 1,
            Name = "My Watchlist",
            IsPinned = true,
            PinPosition = 2,
            TrackedChangeDurationTradingDays = 5,
            WatchedSecurities =
            [
                new() {
                    SecurityId = 1,
                    Name = "Some Company",
                    Symbol = "SOCO",
                    PreviousClosePrice = 234.5
                }
            ]
        };
        var apiModel = Models.Watchlist.Watchlist.FromDomainEntityWithoutCollections(domainWatchlist);
        Assert.NotNull(apiModel);
        Assert.Equal(domainWatchlist.Id, apiModel.Id);
        Assert.Equal(domainWatchlist.Name, apiModel.Name);
        Assert.Equal(domainWatchlist.IsPinned, apiModel.IsPinned);
        Assert.Equal(domainWatchlist.PinPosition, apiModel.PinPosition);
        Assert.Equal(domainWatchlist.TrackedChangeDurationTradingDays, apiModel.TrackedChangeDurationTradingDays);
        Assert.Empty(apiModel.WatchedSecurities);
    }

    [Fact]
    public void Watchlist_FromDomainEntity()
    {
        Domain.Watchlists.Watchlist domainWatchlist = new()
        {
            Id = 1,
            Name = "My Watchlist",
            IsPinned = true,
            PinPosition = 2,
            TrackedChangeDurationTradingDays = 5,
            WatchedSecurities =
            [
                new() {
                    SecurityId = 1,
                    Name = "Some Company",
                    Symbol = "SOCO",
                    PreviousClosePrice = 234.5
                }
            ]
        };
        var apiModel = Models.Watchlist.Watchlist.FromDomainEntity(domainWatchlist);
        Assert.NotNull(apiModel);
        Assert.Equal(domainWatchlist.Id, apiModel.Id);
        Assert.Equal(domainWatchlist.Name, apiModel.Name);
        Assert.Equal(domainWatchlist.IsPinned, apiModel.IsPinned);
        Assert.Equal(domainWatchlist.PinPosition, apiModel.PinPosition);
        Assert.Equal(domainWatchlist.TrackedChangeDurationTradingDays, apiModel.TrackedChangeDurationTradingDays);
        Assert.NotEmpty(apiModel.WatchedSecurities);
        var apiWatchedSecurity = apiModel.WatchedSecurities.First();
        var domainWatchedSecurity = domainWatchlist.WatchedSecurities.First();
        Assert.Equal(domainWatchedSecurity.SecurityId, apiWatchedSecurity.SecurityId);
        Assert.Equal(domainWatchedSecurity.Name, apiWatchedSecurity.Name);
        Assert.Equal(domainWatchedSecurity.Symbol, apiWatchedSecurity.Symbol);
        Assert.Equal(domainWatchedSecurity.PreviousClosePrice, apiWatchedSecurity.PreviousClosePrice);
    }

    [Fact]
    public void Watchlist_ToDomainEntityWithoutCollections()
    {
        Models.Watchlist.Watchlist apiModel = new()
        {
            Id = 1,
            Name = "My Watchlist",
            IsPinned = true,
            PinPosition = 2,
            TrackedChangeDurationTradingDays = 5,
            WatchedSecurities =
            [
                new() {
                    SecurityId = 1,
                    Name = "Some Company",
                    Symbol = "SOCO",
                    PreviousClosePrice = 234.5
                }
            ]
        };
        var domainEntity = apiModel.ToDomainEntityWithoutCollections();
        Assert.NotNull(domainEntity);
        Assert.Equal(apiModel.Id, domainEntity.Id);
        Assert.Equal(apiModel.Name, domainEntity.Name);
        Assert.Equal(apiModel.IsPinned, domainEntity.IsPinned);
        Assert.Equal(apiModel.PinPosition, domainEntity.PinPosition);
        Assert.Equal(apiModel.TrackedChangeDurationTradingDays, domainEntity.TrackedChangeDurationTradingDays);
        Assert.Empty(domainEntity.WatchedSecurities);
    }

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
