using Qlarissa.Infrastructure.DB.Entities.Base;
using Qlarissa.Infrastructure.DB.Entities.MarketData;

namespace Qlarissa.Infrastructure.Tests.DB;

public class MapperTests
{
    [Fact]
    public void DailyPrice_UpdateFromDbEntity_DoesNotUpdateDateOrSecurityId()
    {
        DailyPrice p = new()
        {
            Open = 10.0,
            Close = 10.2,
            High = 10.3,
            Low = 10.0,
            Average = 10.1,
            Date = new DateOnly(2022, 8, 23),
            SecurityId = 2
        };

        DailyPrice incomingDbEntity = new()
        {
            Open = 5.0,
            Close = 5.1,
            High = 5.15,
            Low = 5.0,
            Average = 5.05,
            // Date and SecurityId are checked before, thus they will be the same. This is just to guarantee that the implementation does not attempt to update them.
            Date = new DateOnly(9999, 9, 20),
            SecurityId = 4
        };

        p.UpdateFromDbEntity(incomingDbEntity);

        Assert.Equal(p.Open, incomingDbEntity.Open);
        Assert.Equal(p.Close, incomingDbEntity.Close);
        Assert.Equal(p.High, incomingDbEntity.High);
        Assert.Equal(p.Low, incomingDbEntity.Low);
        Assert.Equal(p.Average, incomingDbEntity.Average);
        Assert.NotEqual(p.Date, incomingDbEntity.Date);
        Assert.NotEqual(p.SecurityId, incomingDbEntity.SecurityId);
    }

    [Fact]
    public void Currency_FromDomainEntity()
    {
        Domain.Currency domainEntity = new() { Id = 1, Symbol = "USD", Name = "US Dollar" };
        var dbEntity = Infrastructure.DB.Entities.Currency.FromDomainEntity(domainEntity);
        Assert.Equal(domainEntity.Id, dbEntity.Id);
        Assert.Equal(domainEntity.Symbol, dbEntity.Symbol);
        Assert.Equal(domainEntity.Name, dbEntity.Name);
    }

    [Fact]
    public void Currency_ToDomainEntity()
    {
        Infrastructure.DB.Entities.Currency dbEntity = new() { Id = 1, Symbol = "USD", Name = "US Dollar" };
        var domainEntity = dbEntity.ToDomainEntity();
        Assert.Equal(dbEntity.Id, domainEntity.Id);
        Assert.Equal(dbEntity.Symbol, domainEntity.Symbol);
        Assert.Equal(dbEntity.Name, domainEntity.Name);
    }

    [Fact]
    public void PubliclyTradedSecurityBase_FromDomainEntity()
    {

        Domain.Securities.ETF domainEntity = new() { Id = 9, Name = "iShares S&P500", 
            Currency = new() { Id = 1, Name = "United States Dollar", Symbol = "USD" }, 
            Symbol="ETFSymbol", Price = 666.6, PriceLastUpdatedTime = new(2025,1,1), PriceHistoryLastDataPointDate = new(2024, 12,30),
            PriceHistory = GetSimplePriceHistoryTestData_DomainEntity(),
        };

        PubliclyTradedSecurityBase dbEntity = PubliclyTradedSecurityBase.FromDomainEntity(domainEntity);
        Assert.Equal(domainEntity.Id, dbEntity.Id);
        Assert.Equal(domainEntity.Name, dbEntity.Name);
        Assert.Equal(domainEntity.Currency.Id, dbEntity.CurrencyId);
        Assert.Equal(domainEntity.Symbol, dbEntity.Symbol);
        Assert.Equal(domainEntity.Price, dbEntity.Price);
        Assert.Equal(domainEntity.PriceLastUpdatedTime, dbEntity.PriceLastUpdatedTime);
        Assert.Equal(domainEntity.PriceHistoryLastDataPointDate, dbEntity.PriceHistoryLastDataPointDate);

        for(int i = 0; i < domainEntity.PriceHistory.Count; i++)
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
    public void PubliclyTradedSecurityBase_ToDomainEntity()
    {
        Infrastructure.DB.Entities.Stock dbEntity = new() { Id = 7, Name = "Microsoft",
            CurrencyId = 10, Currency = new() { Id = 10, Symbol = "USD", Name = "US Dollar" },
            Symbol = "MSFT", Price = 500, PriceLastUpdatedTime = new(2025, 1, 15), PriceHistoryLastDataPointDate = new(2025, 1, 1),
        };

        dbEntity.PriceHistory = GetSimplePriceHistoryTestData_DbEntity(dbEntity);

        Domain.Securities.Base.PubliclyTradedSecurityBase domainEntity = dbEntity.ToDomainEntity();

        Assert.Equal(dbEntity.Id, domainEntity.Id);
        Assert.Equal(dbEntity.Name, domainEntity.Name);
        Assert.Equal(dbEntity.Currency.Id, domainEntity.Currency.Id);
        Assert.Equal(dbEntity.Currency.Symbol, domainEntity.Currency.Symbol);
        Assert.Equal(dbEntity.Currency.Name, domainEntity.Currency.Name);
        Assert.Equal(dbEntity.Symbol, domainEntity.Symbol);
        Assert.Equal(dbEntity.Price, domainEntity.Price);
        Assert.Equal(dbEntity.PriceLastUpdatedTime, domainEntity.PriceLastUpdatedTime);
        Assert.Equal(dbEntity.PriceHistoryLastDataPointDate, domainEntity.PriceHistoryLastDataPointDate);

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
    public void ETF_FromDomainEntity()
    {

        Domain.Securities.ETF domainEntity = new()
        {
            Id = 9,
            Name = "iShares S&P500",
            Currency = new() { Id = 1, Name = "United States Dollar", Symbol = "USD" },
            Symbol = "ETFSymbol",
            Price = 666.6,
            PriceLastUpdatedTime = new(2025, 1, 1),
            PriceHistoryLastDataPointDate = new(2024, 12, 30),
            PriceHistory = GetSimplePriceHistoryTestData_DomainEntity(),
            DistributionEvents = GetSimpleDividendPayoutsTestData_DomainEntity(),
            Splits = [new Domain.Securities.MarketData.Split() { Id = 9, Date = new DateOnly(2019, 1, 20), SplitRatio = 2.0 }]
        };

        PubliclyTradedSecurityBase dbEntity =   PubliclyTradedSecurityBase.FromDomainEntity(domainEntity);

        Assert.Equal(domainEntity.Id, dbEntity.Id);
        Assert.Equal(domainEntity.Name, dbEntity.Name);
        Assert.Equal(domainEntity.Currency.Id, dbEntity.CurrencyId);
        Assert.Equal(domainEntity.Symbol, dbEntity.Symbol);
        Assert.Equal(domainEntity.Price, dbEntity.Price);
        Assert.Equal(domainEntity.PriceLastUpdatedTime, dbEntity.PriceLastUpdatedTime);
        Assert.Equal(domainEntity.PriceHistoryLastDataPointDate, dbEntity.PriceHistoryLastDataPointDate);

        for (int i = 0; i < domainEntity.PriceHistory.Count; i++)
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

        for (int i = 0; i < domainEntity.DistributionEvents.Count; i++)
        {
            Assert.Equal(domainEntity.DistributionEvents[i].Id, dbEntity.DividendPayouts.ElementAt(i).Id);
            Assert.Equal(domainEntity.DistributionEvents[i].PayoutAmount, dbEntity.DividendPayouts.ElementAt(i).PayoutAmount);
            Assert.Equal(domainEntity.DistributionEvents[i].PayoutDate, dbEntity.DividendPayouts.ElementAt(i).PayoutDate);
            Assert.Equal(domainEntity.Id, dbEntity.DividendPayouts.ElementAt(i).SecurityId);
        }

        Assert.NotEmpty(dbEntity.Splits);
        Assert.Equal(domainEntity.Splits.First().Id, dbEntity.Splits.First().Id);
        Assert.Equal(domainEntity.Splits.First().Date, dbEntity.Splits.First().Date);
        Assert.Equal(domainEntity.Splits.First().SplitRatio, dbEntity.Splits.First().SplitRatio);
        Assert.Equal(domainEntity.Id, dbEntity.Splits.First().SecurityId);
    }

    [Fact]
    public void ETF_ToDomainEntity()
    {
        Infrastructure.DB.Entities.ETF dbEntity = new()
        {
            Id = 9,
            Name = "iShares S&P500",
            CurrencyId = 10,
            Currency = new() { Id = 10, Symbol = "USD", Name = "US Dollar" },
            Symbol = "ETFSymbol",
            Price = 666.6,
            PriceLastUpdatedTime = new(2025, 1, 15),
            PriceHistoryLastDataPointDate = new(2025, 1, 1),
        };

        dbEntity.PriceHistory = GetSimplePriceHistoryTestData_DbEntity(dbEntity);
        dbEntity.DividendPayouts = GetSimpleDividendPayoutsTestData_DbEntity(dbEntity);

        Domain.Securities.ETF? domainEntity = dbEntity.ToDomainEntity() as Domain.Securities.ETF;

        Assert.NotNull(domainEntity);
        Assert.Equal(dbEntity.Id, domainEntity.Id);
        Assert.Equal(dbEntity.Name, domainEntity.Name);
        Assert.Equal(dbEntity.Currency.Id, domainEntity.Currency.Id);
        Assert.Equal(dbEntity.Currency.Symbol, domainEntity.Currency.Symbol);
        Assert.Equal(dbEntity.Currency.Name, domainEntity.Currency.Name);
        Assert.Equal(dbEntity.Symbol, domainEntity.Symbol);
        Assert.Equal(dbEntity.Price, domainEntity.Price);
        Assert.Equal(dbEntity.PriceLastUpdatedTime, domainEntity.PriceLastUpdatedTime);
        Assert.Equal(dbEntity.PriceHistoryLastDataPointDate, domainEntity.PriceHistoryLastDataPointDate);

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

    [Fact]
    public void Stock_FromDomainEntity()
    {

        Domain.Securities.Stock domainEntity = new()
        {
            Id = 9,
            Name = "Microsoft",
            Currency = new() { Id = 1, Name = "United States Dollar", Symbol = "USD" },
            Symbol = "MSFT",
            Price = 555.5,
            PriceLastUpdatedTime = new(2025, 1, 1),
            PriceHistoryLastDataPointDate = new(2024, 12, 30),
            PriceHistory = GetSimplePriceHistoryTestData_DomainEntity(),
            DividendPayouts = GetSimpleDividendPayoutsTestData_DomainEntity(),
            InvestorRelationsURL = "https://www.microsoft.com/en-us/investor/default"
        };

        Infrastructure.DB.Entities.Stock? dbEntity = PubliclyTradedSecurityBase.FromDomainEntity(domainEntity) as Infrastructure.DB.Entities.Stock;

        Assert.NotNull(dbEntity);
        Assert.Equal(domainEntity.Id, dbEntity.Id);
        Assert.Equal(domainEntity.Name, dbEntity.Name);
        Assert.Equal(domainEntity.Currency.Id, dbEntity.CurrencyId);
        Assert.Equal(domainEntity.Symbol, dbEntity.Symbol);
        Assert.Equal(domainEntity.Price, dbEntity.Price);
        Assert.Equal(domainEntity.PriceLastUpdatedTime, dbEntity.PriceLastUpdatedTime);
        Assert.Equal(domainEntity.PriceHistoryLastDataPointDate, dbEntity.PriceHistoryLastDataPointDate);
        Assert.Equal(domainEntity.InvestorRelationsURL, dbEntity.InvestorRelationsURL);

        for (int i = 0; i < domainEntity.PriceHistory.Count; i++)
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

        for (int i = 0; i < domainEntity.DividendPayouts.Count; i++)
        {
            Assert.Equal(domainEntity.DividendPayouts[i].Id, dbEntity.DividendPayouts.ElementAt(i).Id);
            Assert.Equal(domainEntity.DividendPayouts[i].PayoutAmount, dbEntity.DividendPayouts.ElementAt(i).PayoutAmount);
            Assert.Equal(domainEntity.DividendPayouts[i].PayoutDate, dbEntity.DividendPayouts.ElementAt(i).PayoutDate);
            Assert.Equal(domainEntity.Id, dbEntity.DividendPayouts.ElementAt(i).SecurityId);
        }
    }

    [Fact]
    public void Stock_ToDomainEntity()
    {
        Infrastructure.DB.Entities.Stock dbEntity = new()
        {
            Id = 9,
            Name = "Microsoft",
            CurrencyId = 10,
            Currency = new() { Id = 10, Symbol = "USD", Name = "US Dollar" },
            Symbol = "MSFT",
            Price = 555.5,
            PriceLastUpdatedTime = new(2025, 1, 15),
            PriceHistoryLastDataPointDate = new(2025, 1, 1),
            InvestorRelationsURL = "https://www.microsoft.com/en-us/investor/default"
        };

        dbEntity.PriceHistory = GetSimplePriceHistoryTestData_DbEntity(dbEntity);
        dbEntity.DividendPayouts = GetSimpleDividendPayoutsTestData_DbEntity(dbEntity);

        Domain.Securities.Stock? domainEntity = dbEntity.ToDomainEntity() as Domain.Securities.Stock;

        Assert.NotNull(domainEntity);
        Assert.Equal(dbEntity.Id, domainEntity.Id);
        Assert.Equal(dbEntity.Name, domainEntity.Name);
        Assert.Equal(dbEntity.Currency.Id, domainEntity.Currency.Id);
        Assert.Equal(dbEntity.Currency.Symbol, domainEntity.Currency.Symbol);
        Assert.Equal(dbEntity.Currency.Name, domainEntity.Currency.Name);
        Assert.Equal(dbEntity.Symbol, domainEntity.Symbol);
        Assert.Equal(dbEntity.Price, domainEntity.Price);
        Assert.Equal(dbEntity.PriceLastUpdatedTime, domainEntity.PriceLastUpdatedTime);
        Assert.Equal(dbEntity.PriceHistoryLastDataPointDate, domainEntity.PriceHistoryLastDataPointDate);
        Assert.Equal(dbEntity.InvestorRelationsURL, domainEntity.InvestorRelationsURL);

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
            Assert.Equal(dbEntity.DividendPayouts.ElementAt(i).Id, domainEntity.DividendPayouts[i].Id);
            Assert.Equal(dbEntity.DividendPayouts.ElementAt(i).PayoutAmount, domainEntity.DividendPayouts[i].PayoutAmount);
            Assert.Equal(dbEntity.DividendPayouts.ElementAt(i).PayoutDate, domainEntity.DividendPayouts[i].PayoutDate);
            Assert.Equal(dbEntity.DividendPayouts.ElementAt(i).SecurityId, domainEntity.Id);
        }
    }

    private static Domain.Securities.MarketData.DailyPrice[] GetSimplePriceHistoryTestData_DomainEntity()
    {
        Domain.Securities.MarketData.DailyPrice[] priceHistory = [
            new() { Id = 100, Date = new(2024, 12, 1), Average = 630, Close = 635, Open = 625, High = 627, Low = 624 },
            new() { Id = 101, Date = new(2024, 12, 2), Average = 631, Close = 636, Open = 626, High = 628, Low = 625 },
            new() { Id = 102, Date = new(2024, 12, 3), Average = 632, Close = 637, Open = 627, High = 629, Low = 626 },
            new() { Id = 103, Date = new(2024, 12, 4), Average = 633, Close = 638, Open = 628, High = 630, Low = 627 }];
        return priceHistory;
    }

    private static DailyPrice[] GetSimplePriceHistoryTestData_DbEntity(PubliclyTradedSecurityBase dbEntity)
    {
        DailyPrice[] priceHistory = [
            new() { Id = 100, Date = new(2024, 12, 1), Average = 630, Close = 635, Open = 625, High = 627, Low = 624, Security = dbEntity, SecurityId = dbEntity.Id },
            new() { Id = 101, Date = new(2024, 12, 2), Average = 631, Close = 636, Open = 626, High = 628, Low = 625, Security = dbEntity, SecurityId = dbEntity.Id },
            new() { Id = 102, Date = new(2024, 12, 3), Average = 632, Close = 637, Open = 627, High = 629, Low = 626, Security = dbEntity, SecurityId = dbEntity.Id },
            new() { Id = 103, Date = new(2024, 12, 4), Average = 633, Close = 638, Open = 628, High = 630, Low = 627, Security = dbEntity, SecurityId = dbEntity.Id }];
        return priceHistory;
    }

    private static Domain.Securities.MarketData.DividendPayout[] GetSimpleDividendPayoutsTestData_DomainEntity()
    {
        Domain.Securities.MarketData.DividendPayout[] payouts = [
            new() { Id = 1, PayoutDate = new(2024,06, 24), PayoutAmount = 6 },
            new() { Id = 2, PayoutDate = new(2024,09, 27), PayoutAmount = 6.2 },
            new() { Id = 3, PayoutDate = new(2024,12, 30), PayoutAmount = 6.5 },
            ];
        return payouts;
    }

    private static DividendPayout[] GetSimpleDividendPayoutsTestData_DbEntity(PubliclyTradedSecurityBase dbEntity)
    {
        DividendPayout[] payouts = [
            new() { Id = 1, PayoutDate = new(2024,06, 24), PayoutAmount = 6, Security = dbEntity, SecurityId = dbEntity.Id },
            new() { Id = 2, PayoutDate = new(2024,09, 27), PayoutAmount = 6.2, Security = dbEntity, SecurityId = dbEntity.Id },
            new() { Id = 3, PayoutDate = new(2024,12, 30), PayoutAmount = 6.5, Security = dbEntity, SecurityId = dbEntity.Id },
            ];
        return payouts;
    }
}