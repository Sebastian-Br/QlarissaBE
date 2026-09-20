using Qlarissa.WebAPI.Models.Watchlist;

namespace Qlarissa.WebAPI.Tests;

public class SecurityModelsTests
{
    [Fact]
    public void DailyPrices_FromDomainEntity()
    {
        Domain.Securities.MarketData.DailyPrice domainEntity = new Domain.Securities.MarketData.DailyPrice()
        {
            Id = 1,
            Open = 10.0,
            Close = 9.9,
            High = 10.24,
            Low = 9.89,
            Average = 9.95,
            Date = new DateOnly(2022, 8, 30)
        };

        var apiModel = Models.Security.MarketData.DailyPrice.FromDomainEntity(domainEntity);

        Assert.NotNull(apiModel);
        Assert.Equal(domainEntity.Id, apiModel.Id);
        Assert.Equal(domainEntity.Open, apiModel.Open);
        Assert.Equal(domainEntity.Close, apiModel.Close);
        Assert.Equal(domainEntity.High, apiModel.High);
        Assert.Equal(domainEntity.Low, apiModel.Low);
        Assert.Equal(domainEntity.Average, apiModel.Average);
        Assert.Equal(domainEntity.Date, apiModel.Date);
    }

    [Fact]
    public void DividendPayout_FromDomainEntity()
    {
        Domain.Securities.MarketData.DividendPayout domainEntity = new()
        {
            Id = 1,
            PayoutDate = new DateOnly(2022, 8, 30),
            PayoutAmount = 1.2
        };

        var apiModel = Models.Security.MarketData.DividendPayout.FromDomainEntity(domainEntity);

        Assert.NotNull(apiModel);
        Assert.Equal(domainEntity.Id, apiModel.Id);
        Assert.Equal(domainEntity.PayoutDate, apiModel.PayoutDate);
        Assert.Equal(domainEntity.PayoutAmount, apiModel.PayoutAmount);
    }

    [Fact]
    public void Split_FromDomainEntity()
    {
        Domain.Securities.MarketData.Split domainEntity = new()
        {
            Id = 1,
            Date = new DateOnly(2022, 8, 30),
            SplitRatio = 2.0
        };

        var apiModel = Models.Security.MarketData.Split.FromDomainEntity(domainEntity);

        Assert.NotNull(apiModel);
        Assert.Equal(domainEntity.Id, apiModel.Id);
        Assert.Equal(domainEntity.Date, apiModel.Date);
        Assert.Equal(domainEntity.SplitRatio, apiModel.SplitRatio);
    }

    [Fact]
    public void WatchedSecurityMinimal_FromDomainEntity()
    {
        Domain.WatchedSecurityMinimal domainEntity = new()
        {
            IsWatched = true,
            IsOnPrimaryWatchlist = false
        };

        var apiModel = WatchedSecurityMinimal.FromDomainEntity(domainEntity);

        Assert.Equal(domainEntity.IsWatched, apiModel.IsWatched);
        Assert.Equal(domainEntity.IsOnPrimaryWatchlist, apiModel.IsOnPrimaryWatchlist);
    }

    [Fact]
    public void WatchedSecurity_FromDomainEntity()
    {
        Domain.WatchedSecurity domainEntity = new()
        {
            SecurityId = 1,
            Name = "Some Company",
            Symbol = "SOCO",
            PreviousDaysClosePrice = 234.5
        };

        var apiModel = Models.Watchlist.WatchedSecurity.FromDomainEntity(domainEntity);

        Assert.NotNull(apiModel);
        Assert.Equal(domainEntity.SecurityId, apiModel.SecurityId);
        Assert.Equal(domainEntity.Name, apiModel.Name);
        Assert.Equal(domainEntity.Symbol, apiModel.Symbol);
        Assert.Equal(domainEntity.PreviousDaysClosePrice, apiModel.PreviousDaysClosePrice);
    }

    [Fact]
    public void UserWatchlists_FromDomainEntity_ShouldContainEmptyLists()
    {
        Domain.WatchList[] domainWatchlists = [];

        var apiModel = UserWatchlists.FromDomainEntity(domainWatchlists);

        Assert.Empty(apiModel.PrimaryWatchlist);
        Assert.Empty(apiModel.SecondaryWatchlist);
    }

    [Fact]
    public void UserWatchlists_FromDomainEntity()
    {
        Domain.WatchList[] domainWatchlists = [
                new()
                {
                    IsPrimary = true,
                    WatchedSecurities = [
                           new()
                           {
                               Name = "Company 1 Inc.",
                               Symbol = "CO1",
                               SecurityId = 4,
                               PreviousDaysClosePrice = 109.5
                           },
                           new()
                           {
                               Name = "ETF 1",
                               Symbol = "ETF1",
                               SecurityId = 5,
                               PreviousDaysClosePrice = 55.8
                           }
                        ]
                },
                new()
                {
                    IsPrimary = false,
                    WatchedSecurities = [
                           new()
                           {
                               Name = "Currency 1",
                               Symbol = "C1",
                               SecurityId = 7,
                               PreviousDaysClosePrice = 1.4
                           },
                           new()
                           {
                               Name = "Cryptocurrency 1",
                               Symbol = "CR1",
                               SecurityId = 12,
                               PreviousDaysClosePrice = 2789
                           }
                        ]
                }
            ];

        var apiModel = UserWatchlists.FromDomainEntity(domainWatchlists);

        Assert.NotEmpty(apiModel.PrimaryWatchlist);
        Assert.NotEmpty(apiModel.SecondaryWatchlist);
        Assert.Equal(apiModel.PrimaryWatchlist[0].Name, domainWatchlists[0].WatchedSecurities.ElementAt(0).Name);
        Assert.Equal(apiModel.PrimaryWatchlist[0].Symbol, domainWatchlists[0].WatchedSecurities.ElementAt(0).Symbol);
        Assert.Equal(apiModel.PrimaryWatchlist[0].SecurityId, domainWatchlists[0].WatchedSecurities.ElementAt(0).SecurityId);
        Assert.Equal(apiModel.PrimaryWatchlist[0].PreviousDaysClosePrice, domainWatchlists[0].WatchedSecurities.ElementAt(0).PreviousDaysClosePrice);

        Assert.Equal(apiModel.PrimaryWatchlist[1].Name, domainWatchlists[0].WatchedSecurities.ElementAt(1).Name);
        Assert.Equal(apiModel.PrimaryWatchlist[1].Symbol, domainWatchlists[0].WatchedSecurities.ElementAt(1).Symbol);
        Assert.Equal(apiModel.PrimaryWatchlist[1].SecurityId, domainWatchlists[0].WatchedSecurities.ElementAt(1).SecurityId);
        Assert.Equal(apiModel.PrimaryWatchlist[1].PreviousDaysClosePrice, domainWatchlists[0].WatchedSecurities.ElementAt(1).PreviousDaysClosePrice);

        Assert.Equal(apiModel.SecondaryWatchlist[0].Name, domainWatchlists[1].WatchedSecurities.ElementAt(0).Name);
        Assert.Equal(apiModel.SecondaryWatchlist[0].Symbol, domainWatchlists[1].WatchedSecurities.ElementAt(0).Symbol);
        Assert.Equal(apiModel.SecondaryWatchlist[0].SecurityId, domainWatchlists[1].WatchedSecurities.ElementAt(0).SecurityId);
        Assert.Equal(apiModel.SecondaryWatchlist[0].PreviousDaysClosePrice, domainWatchlists[1].WatchedSecurities.ElementAt(0).PreviousDaysClosePrice);

        Assert.Equal(apiModel.SecondaryWatchlist[1].Name, domainWatchlists[1].WatchedSecurities.ElementAt(1).Name);
        Assert.Equal(apiModel.SecondaryWatchlist[1].Symbol, domainWatchlists[1].WatchedSecurities.ElementAt(1).Symbol);
        Assert.Equal(apiModel.SecondaryWatchlist[1].SecurityId, domainWatchlists[1].WatchedSecurities.ElementAt(1).SecurityId);
        Assert.Equal(apiModel.SecondaryWatchlist[1].PreviousDaysClosePrice, domainWatchlists[1].WatchedSecurities.ElementAt(1).PreviousDaysClosePrice);
    }

    /// <summary>
    /// Only tests the shared PubliclyTradedSecurityBase properties.
    /// </summary>
    [Fact]
    public void PubliclyTradedSecurityBase_FromDomainEntity()
    {
        Domain.Securities.Stock domainEntity = new()
        {
            Id = 1,
            Name = "Some Company Inc.",
            ShortName = "Some Company",
            Currency = new Domain.Currency()
            {
                Id = 2,
                Symbol = "USD",
                Name = "United States Dollar"
            },
            SecurityType = Domain.Securities.Base.SecurityType.Stock,
            ExchangeName = "Exchange",
            ExchangeShortName = "EXCHG",
            Symbol = "SOCO",
            PriceHistory = GeneratePriceHistoryDomainEntity(),
            Price = 102.7,
            PriceLastUpdatedTime = DateTime.UtcNow,
            PriceHistoryLastDataPointDate = new DateOnly(2022, 8, 30)
        };

        var apiModel = Models.Security.Base.PubliclyTradedSecurityBase.FromDomainEntity(domainEntity);

        Assert.NotNull(apiModel);
        Assert.Equal(domainEntity.Id, apiModel.Id);
        Assert.Equal(domainEntity.Name, apiModel.Name);
        Assert.Equal(domainEntity.ShortName, apiModel.ShortName);
        Assert.Equal(domainEntity.Currency.Id, apiModel.Currency.Id);
        Assert.Equal(domainEntity.Currency.Symbol, apiModel.Currency.Symbol);
        Assert.Equal(domainEntity.Currency.Name, apiModel.Currency.Name);
        Assert.Equal((int)domainEntity.SecurityType, (int)apiModel.SecurityType);
        Assert.Equal(domainEntity.ExchangeName, apiModel.ExchangeName);
        Assert.Equal(domainEntity.ExchangeShortName, apiModel.ExchangeShortName);
        Assert.Equal(domainEntity.Symbol, apiModel.Symbol);

        var apiModelPriceHistoryArray = apiModel.PriceHistory.ToArray();
        var domainEntityPriceHistoryArray = domainEntity.PriceHistory.ToArray();

        for (int i = 0; i < domainEntity.PriceHistory.Count; i++)
        {
            Assert.Equal(domainEntityPriceHistoryArray[i].Id, apiModelPriceHistoryArray[i].Id);
            Assert.Equal(domainEntityPriceHistoryArray[i].Open, apiModelPriceHistoryArray[i].Open);
            Assert.Equal(domainEntityPriceHistoryArray[i].Close, apiModelPriceHistoryArray[i].Close);
            Assert.Equal(domainEntityPriceHistoryArray[i].High, apiModelPriceHistoryArray[i].High);
            Assert.Equal(domainEntityPriceHistoryArray[i].Low, apiModelPriceHistoryArray[i].Low);
            Assert.Equal(domainEntityPriceHistoryArray[i].Average, apiModelPriceHistoryArray[i].Average);
            Assert.Equal(domainEntityPriceHistoryArray[i].Date, apiModelPriceHistoryArray[i].Date);
        }

        Assert.Equal(domainEntity.Price, apiModel.Price);
        Assert.Equal(domainEntity.PriceLastUpdatedTime, apiModel.PriceLastUpdatedTime);
    }

    private Domain.Securities.MarketData.DailyPrice[] GeneratePriceHistoryDomainEntity()
    {
        return [
                new()
                {
                    Id = 1,
                    Open = 100.0,
                    Close = 102.0,
                    High = 102.4,
                    Low = 99.9,
                    Average = 101.0,
                    Date = new DateOnly(2022, 8, 29)
                },
                new()
                {
                    Id = 2,
                    Open = 101.0,
                    Close = 102.0,
                    High = 102.4,
                    Low = 100.3,
                    Average = 101.5,
                    Date = new DateOnly(2022, 8, 30)
                }
            ];
    }

    private Domain.Securities.MarketData.DividendPayout[] GenerateDividendHistory()
    {
        return [
                new()
                {
                    Id = 1,
                    PayoutDate = new DateOnly(2022, 3, 22),
                    PayoutAmount = 1.02
                },
                new()
                {
                    Id = 2,
                    PayoutDate = new DateOnly(2022, 6, 22),
                    PayoutAmount = 1.03
                }
            ];
    }

    private Domain.Securities.MarketData.Split[] GenerateSplitHistory()
    {
        return [
                new()
                {
                    Id = 1,
                    Date = new DateOnly(2015, 7, 25),
                    SplitRatio = 10.0
                },
                new()
                {
                    Id = 2,
                    Date = new DateOnly(2025, 11, 24),
                    SplitRatio = 2.0
                }
            ];
    }
}