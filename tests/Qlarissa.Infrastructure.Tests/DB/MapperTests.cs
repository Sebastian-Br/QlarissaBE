
using Qlarissa.Infrastructure.DB.Entities.Base;

namespace Qlarissa.Infrastructure.Tests.DB;

public class MapperTests
{
    [Fact]
    public void MapCurrency_FromDomainEntity()
    {
        Domain.Entities.Securities.Currency domainEntity = new() { Id = 1, Symbol = "USD", Name = "US Dollar" };
        var dbEntity = Infrastructure.DB.Entities.Currency.FromDomainEntity(domainEntity);
        Assert.Equal(domainEntity.Id, dbEntity.Id);
        Assert.Equal(domainEntity.Symbol, dbEntity.Symbol);
        Assert.Equal(domainEntity.Name, dbEntity.Name);
    }

    [Fact]
    public void MapCurrency_ToDomainEntity()
    {
        Infrastructure.DB.Entities.Currency dbEntity = new() { Id = 1, Symbol = "USD", Name = "US Dollar" };
        var domainEntity = dbEntity.ToDomainEntity();
        Assert.Equal(dbEntity.Id, domainEntity.Id);
        Assert.Equal(dbEntity.Symbol, domainEntity.Symbol);
        Assert.Equal(dbEntity.Name, domainEntity.Name);
    }

    /// <summary>
    /// Not testing dividend history - those will be tested for Stock/ETF entities
    /// </summary>
    [Fact]
    public void MapPubliclyTradedSecurityBase_FromDomainEntity()
    {
        
        Domain.Entities.Securities.ETF domainEntity = new() { Id = 9, Name = "iShares S&P500", 
            Currency = new() { Id = 1, Name = "United States Dollar", Symbol = "USD" }, 
            Symbol="ETFSymbol", Price = 666.6m, PriceLastUpdatedTime = new(2025,1,1), LastCompleteUpdateTime = new(2024, 12,30),
            PriceHistory = GetSimplePriceHistoryTestData(),
            // DistributionEvents = GetSimpleDividendPayoutsTestData() -- tested on Stock/ETF entities.
        };

        PubliclyTradedSecurityBase dbEntity = new();
        PubliclyTradedSecurityBase.FromDomainEntity(domainEntity, dbEntity);
        Assert.Equal(domainEntity.Id, dbEntity.Id);
        Assert.Equal(domainEntity.Name, dbEntity.Name);
        Assert.Equal(domainEntity.Currency.Id, dbEntity.CurrencyId);
        Assert.Equal(domainEntity.Symbol, dbEntity.Symbol);
        Assert.Equal(domainEntity.Price, dbEntity.Price);
        Assert.Equal(domainEntity.PriceLastUpdatedTime, dbEntity.PriceLastUpdatedTime);
        Assert.Equal(domainEntity.LastCompleteUpdateTime, dbEntity.LastCompleteUpdateTime);

        for(int i = 0; i < domainEntity.PriceHistory.Length; i++)
        {
            Assert.Equal(dbEntity.PriceHistory.ElementAt(i).Id, domainEntity.PriceHistory[i].Id);
            Assert.Equal(dbEntity.PriceHistory.ElementAt(i).Low, domainEntity.PriceHistory[i].Low);
            Assert.Equal(dbEntity.PriceHistory.ElementAt(i).High, domainEntity.PriceHistory[i].High);
            Assert.Equal(dbEntity.PriceHistory.ElementAt(i).Open, domainEntity.PriceHistory[i].Open);
            Assert.Equal(dbEntity.PriceHistory.ElementAt(i).Close, domainEntity.PriceHistory[i].Close);
            Assert.Equal(dbEntity.PriceHistory.ElementAt(i).Average, domainEntity.PriceHistory[i].Average);
            Assert.Equal(dbEntity.PriceHistory.ElementAt(i).Date, domainEntity.PriceHistory[i].Date);
            Assert.Equal(dbEntity.PriceHistory.ElementAt(i).SecurityId, domainEntity.Id);
        }
        
    }

    private static Domain.Entities.Securities.MarketData.DailyPrice[] GetSimplePriceHistoryTestData()
    {
        Domain.Entities.Securities.MarketData.DailyPrice[] priceHistory = [
            new() { Id = 100, Date = new(2024, 12, 1), Average = 630, Close = 635, Open = 625, High = 627, Low = 624 },
            new() { Id = 101, Date = new(2024, 12, 2), Average = 631, Close = 636, Open = 626, High = 628, Low = 625 },
            new() { Id = 102, Date = new(2024, 12, 3), Average = 632, Close = 637, Open = 627, High = 629, Low = 626 },
            new() { Id = 103, Date = new(2024, 12, 4), Average = 633, Close = 638, Open = 628, High = 630, Low = 627 }];
        return priceHistory;
    }

    private static Domain.Entities.Securities.MarketData.DividendPayout[] GetSimpleDividendPayoutsTestData()
    {
        Domain.Entities.Securities.MarketData.DividendPayout[] payouts = [
            new() { PayoutDate = new(2024,06, 24), PayoutAmount = 6 },
            new() { PayoutDate = new(2024,09, 27), PayoutAmount = 6.2m },
            new() { PayoutDate = new(2024,12, 30), PayoutAmount = 6.5m },
            ];
        return payouts;
    }
}