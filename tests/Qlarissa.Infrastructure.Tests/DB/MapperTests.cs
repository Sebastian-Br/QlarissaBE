
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
            PriceHistory = GetSimplePriceHistoryTestData_DomainEntity(),
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
            Assert.Equal(domainEntity.PriceHistory[i].Id, dbEntity.PriceHistory.ElementAt(i).Id);
            Assert.Equal(domainEntity.PriceHistory[i].Low, dbEntity.PriceHistory.ElementAt(i).Low);
            Assert.Equal(domainEntity.PriceHistory[i].High, dbEntity.PriceHistory.ElementAt(i).High);
            Assert.Equal(domainEntity.PriceHistory[i].Open, dbEntity.PriceHistory.ElementAt(i).Open);
            Assert.Equal(domainEntity.PriceHistory[i].Close, dbEntity.PriceHistory.ElementAt(i).Close);
            Assert.Equal(domainEntity.PriceHistory[i].Average, dbEntity.PriceHistory.ElementAt(i).Average);
            Assert.Equal(domainEntity.PriceHistory[i].Date, dbEntity.PriceHistory.ElementAt(i).Date);
            Assert.Equal(domainEntity.Id, dbEntity.PriceHistory.ElementAt(i).SecurityId);
        }
    }

    [Fact]
    public void MapPubliclyTradedSecurityBase_ToDomainEntity()
    {
        Infrastructure.DB.Entities.Stock dbEntity = new() { Id = 7, Name = "Microsoft",
            CurrencyId = 10, Currency = new() { Id = 10, Symbol = "USD", Name = "US Dollar" },
            Symbol = "MSFT", Price = 500, PriceLastUpdatedTime = new(2025, 1, 15), LastCompleteUpdateTime = new(2025, 1, 1),
        };

        dbEntity.PriceHistory = GetSimplePriceHistoryTestData_DbEntity(dbEntity);

        Domain.Entities.Securities.Stock domainEntity = new();
        PubliclyTradedSecurityBase.ToDomainEntity(domainEntity, dbEntity);

        Assert.Equal(dbEntity.Id, domainEntity.Id);
        Assert.Equal(dbEntity.Name, domainEntity.Name);
        Assert.Equal(dbEntity.Currency.Id, domainEntity.Currency.Id);
        Assert.Equal(dbEntity.Currency.Symbol, domainEntity.Currency.Symbol);
        Assert.Equal(dbEntity.Currency.Name, domainEntity.Currency.Name);
        Assert.Equal(dbEntity.Symbol, domainEntity.Symbol);
        Assert.Equal(dbEntity.Price, domainEntity.Price);
        Assert.Equal(dbEntity.PriceLastUpdatedTime, domainEntity.PriceLastUpdatedTime);
        Assert.Equal(dbEntity.LastCompleteUpdateTime, domainEntity.LastCompleteUpdateTime);

        for (int i = 0; i < dbEntity.PriceHistory.Count; i++)
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

    [Fact]
    public void MapETF_FromDomainEntity()
    {

        Domain.Entities.Securities.ETF domainEntity = new()
        {
            Id = 9,
            Name = "iShares S&P500",
            Currency = new() { Id = 1, Name = "United States Dollar", Symbol = "USD" },
            Symbol = "ETFSymbol",
            Price = 666.6m,
            PriceLastUpdatedTime = new(2025, 1, 1),
            LastCompleteUpdateTime = new(2024, 12, 30),
            PriceHistory = GetSimplePriceHistoryTestData_DomainEntity(),
            DistributionEvents = GetSimpleDividendPayoutsTestData_DomainEntity()
        };

        Infrastructure.DB.Entities.ETF dbEntity = Infrastructure.DB.Entities.ETF.FromDomainEntity(domainEntity);

        Assert.Equal(domainEntity.Id, dbEntity.Id);
        Assert.Equal(domainEntity.Name, dbEntity.Name);
        Assert.Equal(domainEntity.Currency.Id, dbEntity.CurrencyId);
        Assert.Equal(domainEntity.Symbol, dbEntity.Symbol);
        Assert.Equal(domainEntity.Price, dbEntity.Price);
        Assert.Equal(domainEntity.PriceLastUpdatedTime, dbEntity.PriceLastUpdatedTime);
        Assert.Equal(domainEntity.LastCompleteUpdateTime, dbEntity.LastCompleteUpdateTime);

        for (int i = 0; i < domainEntity.PriceHistory.Length; i++)
        {
            Assert.Equal(domainEntity.PriceHistory[i].Id, dbEntity.PriceHistory.ElementAt(i).Id);
            Assert.Equal(domainEntity.PriceHistory[i].Low, dbEntity.PriceHistory.ElementAt(i).Low);
            Assert.Equal(domainEntity.PriceHistory[i].High, dbEntity.PriceHistory.ElementAt(i).High);
            Assert.Equal(domainEntity.PriceHistory[i].Open, dbEntity.PriceHistory.ElementAt(i).Open);
            Assert.Equal(domainEntity.PriceHistory[i].Close, dbEntity.PriceHistory.ElementAt(i).Close);
            Assert.Equal(domainEntity.PriceHistory[i].Average, dbEntity.PriceHistory.ElementAt(i).Average);
            Assert.Equal(domainEntity.PriceHistory[i].Date, dbEntity.PriceHistory.ElementAt(i).Date);
            Assert.Equal(domainEntity.Id, dbEntity.PriceHistory.ElementAt(i).SecurityId);
        }

        for (int i = 0; i < domainEntity.DistributionEvents.Length; i++)
        {
            Assert.Equal(domainEntity.DistributionEvents[i].Id, dbEntity.DividendPayouts.ElementAt(i).Id);
            Assert.Equal(domainEntity.DistributionEvents[i].PayoutAmount, dbEntity.DividendPayouts.ElementAt(i).PayoutAmount);
            Assert.Equal(domainEntity.DistributionEvents[i].PayoutDate, dbEntity.DividendPayouts.ElementAt(i).PayoutDate);
            Assert.Equal(domainEntity.Id, dbEntity.DividendPayouts.ElementAt(i).SecurityId);
        }
    }

    [Fact]
    public void MapETF_ToDomainEntity()
    {
        Infrastructure.DB.Entities.ETF dbEntity = new()
        {
            Id = 9,
            Name = "iShares S&P500",
            CurrencyId = 10,
            Currency = new() { Id = 10, Symbol = "USD", Name = "US Dollar" },
            Symbol = "ETFSymbol",
            Price = 666.6m,
            PriceLastUpdatedTime = new(2025, 1, 15),
            LastCompleteUpdateTime = new(2025, 1, 1),
        };

        dbEntity.PriceHistory = GetSimplePriceHistoryTestData_DbEntity(dbEntity);
        dbEntity.DividendPayouts = GetSimpleDividendPayoutsTestData_DbEntity(dbEntity);

        Domain.Entities.Securities.ETF domainEntity = dbEntity.ToDomainEntity();

        Assert.Equal(dbEntity.Id, domainEntity.Id);
        Assert.Equal(dbEntity.Name, domainEntity.Name);
        Assert.Equal(dbEntity.Currency.Id, domainEntity.Currency.Id);
        Assert.Equal(dbEntity.Currency.Symbol, domainEntity.Currency.Symbol);
        Assert.Equal(dbEntity.Currency.Name, domainEntity.Currency.Name);
        Assert.Equal(dbEntity.Symbol, domainEntity.Symbol);
        Assert.Equal(dbEntity.Price, domainEntity.Price);
        Assert.Equal(dbEntity.PriceLastUpdatedTime, domainEntity.PriceLastUpdatedTime);
        Assert.Equal(dbEntity.LastCompleteUpdateTime, domainEntity.LastCompleteUpdateTime);

        for (int i = 0; i < dbEntity.PriceHistory.Count; i++)
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

        for (int i = 0; i < dbEntity.DividendPayouts.Count; i++)
        {
            Assert.Equal(dbEntity.DividendPayouts.ElementAt(i).Id, domainEntity.DistributionEvents[i].Id);
            Assert.Equal(dbEntity.DividendPayouts.ElementAt(i).PayoutAmount, domainEntity.DistributionEvents[i].PayoutAmount);
            Assert.Equal(dbEntity.DividendPayouts.ElementAt(i).PayoutDate, domainEntity.DistributionEvents[i].PayoutDate);
            Assert.Equal(dbEntity.DividendPayouts.ElementAt(i).SecurityId, domainEntity.Id);
        }
    }

    private static Domain.Entities.Securities.MarketData.DailyPrice[] GetSimplePriceHistoryTestData_DomainEntity()
    {
        Domain.Entities.Securities.MarketData.DailyPrice[] priceHistory = [
            new() { Id = 100, Date = new(2024, 12, 1), Average = 630, Close = 635, Open = 625, High = 627, Low = 624 },
            new() { Id = 101, Date = new(2024, 12, 2), Average = 631, Close = 636, Open = 626, High = 628, Low = 625 },
            new() { Id = 102, Date = new(2024, 12, 3), Average = 632, Close = 637, Open = 627, High = 629, Low = 626 },
            new() { Id = 103, Date = new(2024, 12, 4), Average = 633, Close = 638, Open = 628, High = 630, Low = 627 }];
        return priceHistory;
    }

    private static ICollection<Infrastructure.DB.Entities.DailyPrice> GetSimplePriceHistoryTestData_DbEntity(PubliclyTradedSecurityBase dbEntity)
    {
        Infrastructure.DB.Entities.DailyPrice[] priceHistory = [
            new() { Id = 100, Date = new(2024, 12, 1), Average = 630, Close = 635, Open = 625, High = 627, Low = 624, Security = dbEntity, SecurityId = dbEntity.Id },
            new() { Id = 101, Date = new(2024, 12, 2), Average = 631, Close = 636, Open = 626, High = 628, Low = 625, Security = dbEntity, SecurityId = dbEntity.Id },
            new() { Id = 102, Date = new(2024, 12, 3), Average = 632, Close = 637, Open = 627, High = 629, Low = 626, Security = dbEntity, SecurityId = dbEntity.Id },
            new() { Id = 103, Date = new(2024, 12, 4), Average = 633, Close = 638, Open = 628, High = 630, Low = 627, Security = dbEntity, SecurityId = dbEntity.Id }];
        return priceHistory;
    }

    private static Domain.Entities.Securities.MarketData.DividendPayout[] GetSimpleDividendPayoutsTestData_DomainEntity()
    {
        Domain.Entities.Securities.MarketData.DividendPayout[] payouts = [
            new() { Id = 1, PayoutDate = new(2024,06, 24), PayoutAmount = 6 },
            new() { Id = 2, PayoutDate = new(2024,09, 27), PayoutAmount = 6.2m },
            new() { Id = 3, PayoutDate = new(2024,12, 30), PayoutAmount = 6.5m },
            ];
        return payouts;
    }

    private static ICollection<Infrastructure.DB.Entities.MarketData.DividendPayout> GetSimpleDividendPayoutsTestData_DbEntity(PubliclyTradedSecurityBase dbEntity)
    {
        ICollection<Infrastructure.DB.Entities.MarketData.DividendPayout> payouts = [
            new() { Id = 1, PayoutDate = new(2024,06, 24), PayoutAmount = 6, Security = dbEntity, SecurityId = dbEntity.Id },
            new() { Id = 2, PayoutDate = new(2024,09, 27), PayoutAmount = 6.2m, Security = dbEntity, SecurityId = dbEntity.Id },
            new() { Id = 3, PayoutDate = new(2024,12, 30), PayoutAmount = 6.5m, Security = dbEntity, SecurityId = dbEntity.Id },
            ];
        return payouts;
    }
}