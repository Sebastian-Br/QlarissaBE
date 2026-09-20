using Qlarissa.Infrastructure.PyFinance;

namespace Qlarissa.Infrastructure.Tests.PyFinance;

public class MapperTests
{
    [Fact]
    public void DailyPrice_ToDomainEntity()
    {
        DailyPrice extApiModel = new()
        {
            Open = 100.0,
            Close = 98.0,
            High = 101.0,
            Low = 97.8,
            Date = new DateOnly(2026, 7, 21)
        };

        var domainEntity = extApiModel.ToDomainEntity();

        Assert.Equal(extApiModel.Open, domainEntity.Open);
        Assert.Equal(extApiModel.Close, domainEntity.Close);
        Assert.Equal(extApiModel.High, domainEntity.High);
        Assert.Equal(extApiModel.Low, domainEntity.Low);
        Assert.Equal(extApiModel.Date, domainEntity.Date);
        Assert.Equal((extApiModel.Open + extApiModel.Close) / 2, domainEntity.Average);
    }

    [Fact]
    public void Security_ToDomainEntity_UnsupportedQuoteType_ShouldThrow()
    {
        Security extApiModel = new()
        {
            History = [],
            Info = new()
            {
                QuoteType = "SomeNewQuoteType"
            }
        };

        Assert.Throws<NotSupportedException>(extApiModel.ToDomainEntity);
    }

    [Fact]
    public void Security_ToDomainEntity_Stock()
    {
        Security extApiModel = new()
        {
            ISIN = "US123456000",
            History = GetDailyPricesTestData_ForDividendPayingQuoteTypes(),
            Info = new()
            {
                QuoteType = QuoteType.Stock,
                Symbol = "MSFT",
                LongName = "Microsoft Corporation",
                ShortName = "Microsoft",
                LongBusinessSummary = "Does things.",
                FullDayPrice = 111.1,
                Currency = "USD",
                Exchange = "EXCHG",
                FullExchangeName = "EXCHANGE",
                MarketState = "POST",
                MarketCap = 1000000000000,
                SharesOutstanding = 9000000000,
                DividendRate = 0.1,
                DividendYield = 0,
                IrWebsite = "www...MicrosoftInvestorRelationsURL",
                TargetMeanPrice = 120,
                RecommendationMean = 1.5,
                NetExpenseRatio = 0
            }
        };

        var domainEntity = extApiModel.ToDomainEntity() as Domain.Securities.Stock;

        Assert.NotNull(domainEntity);
        Assert.Equal(Domain.Securities.Base.SecurityType.Stock, domainEntity.SecurityType);
        Assert.Equal(extApiModel.ISIN, domainEntity.ISIN);
        Assert.Equal(extApiModel.Info.Symbol, domainEntity.Symbol);
        Assert.Equal(extApiModel.Info.LongName, domainEntity.Name);
        Assert.Equal(extApiModel.Info.ShortName, domainEntity.ShortName);
        Assert.Equal(extApiModel.Info.LongBusinessSummary, domainEntity.BusinessSummary);
        // MarketCap is better calculated on the fly for Stocks from SharesOutstanding * Price, instead of always updating it from an external API.
        Assert.Equal(extApiModel.Info.SharesOutstanding, domainEntity.SharesOutstanding);
        Assert.Equal(extApiModel.Info.DividendRate, domainEntity.DividendRate);
        // DividendYield is used for ETFs
        Assert.Equal(extApiModel.Info.IrWebsite, domainEntity.InvestorRelationsURL);
        Assert.Equal(extApiModel.Info.TargetMeanPrice, domainEntity.TargetMeanPrice);
        Assert.Equal(extApiModel.Info.RecommendationMean, domainEntity.RecommendationMean);

        var domainEntityPriceHistoryEntries = domainEntity.PriceHistory.ToArray();
        Assert.Equal(2, domainEntityPriceHistoryEntries.Length);

        // Earlier date should come first/should be sorted
        Assert.Equal(new DateOnly(2020, 5, 17), domainEntityPriceHistoryEntries[0].Date);
        Assert.Equal(109, domainEntityPriceHistoryEntries[0].High);
        Assert.Equal(107, domainEntityPriceHistoryEntries[0].Low);
        Assert.Equal(108.1, domainEntityPriceHistoryEntries[0].Open);
        Assert.Equal(108.3, domainEntityPriceHistoryEntries[0].Close);
        Assert.Equal(108.2, domainEntityPriceHistoryEntries[0].Average, precision: 10);

        Assert.Equal(new DateOnly(2020, 5, 20), domainEntityPriceHistoryEntries[1].Date);
        Assert.Equal(114, domainEntityPriceHistoryEntries[1].High);
        Assert.Equal(109, domainEntityPriceHistoryEntries[1].Low);
        Assert.Equal(110, domainEntityPriceHistoryEntries[1].Open);
        Assert.Equal(112, domainEntityPriceHistoryEntries[1].Close);
        Assert.Equal(111, domainEntityPriceHistoryEntries[1].Average, precision: 10);

        Assert.Single(domainEntity.Splits);
        Assert.Equal(new DateOnly(2020, 5, 20), domainEntity.Splits.First().Date);
        Assert.Equal(2.0, domainEntity.Splits.First().SplitRatio);

        Assert.Single(domainEntity.DividendPayouts);
        Assert.Equal(new DateOnly(2020, 5, 17), domainEntity.DividendPayouts[0].PayoutDate);
        Assert.Equal(0.4, domainEntity.DividendPayouts[0].PayoutAmount);
    }

    [Fact]
    public void Security_ToDomainEntity_ETF()
    {
        Security extApiModel = new()
        {
            ISIN = "US123456000",
            History = GetDailyPricesTestData_ForDividendPayingQuoteTypes(),
            Info = new()
            {
                QuoteType = QuoteType.ETF,
                Symbol = "URTH",
                LongName = "MSCI World ETF",
                ShortName = "MSCI World",
                FullDayPrice = 111.1,
                Currency = "USD",
                Exchange = "EXCHG",
                FullExchangeName = "EXCHANGE",
                MarketState = "POST",
                DividendYield = 0.015,
                NetExpenseRatio = 0.2
            }
        };

        var domainEntity = extApiModel.ToDomainEntity() as Domain.Securities.ETF;

        Assert.NotNull(domainEntity);
        Assert.Equal(Domain.Securities.Base.SecurityType.ETF, domainEntity.SecurityType);
        Assert.Equal(extApiModel.ISIN, domainEntity.ISIN);
        Assert.Equal(extApiModel.Info.Symbol, domainEntity.Symbol);
        Assert.Equal(extApiModel.Info.LongName, domainEntity.Name);
        Assert.Equal(extApiModel.Info.ShortName, domainEntity.ShortName);
        Assert.Equal(extApiModel.Info.DividendYield, domainEntity.DividendYield);

        var domainEntityPriceHistoryEntries = domainEntity.PriceHistory.ToArray();
        Assert.Equal(2, domainEntityPriceHistoryEntries.Length);

        // Earlier date should come first/should be sorted
        Assert.Equal(new DateOnly(2020, 5, 17), domainEntityPriceHistoryEntries[0].Date);
        Assert.Equal(109, domainEntityPriceHistoryEntries[0].High);
        Assert.Equal(107, domainEntityPriceHistoryEntries[0].Low);
        Assert.Equal(108.1, domainEntityPriceHistoryEntries[0].Open);
        Assert.Equal(108.3, domainEntityPriceHistoryEntries[0].Close);
        Assert.Equal(108.2, domainEntityPriceHistoryEntries[0].Average, precision: 10);

        Assert.Equal(new DateOnly(2020, 5, 20), domainEntityPriceHistoryEntries[1].Date);
        Assert.Equal(114, domainEntityPriceHistoryEntries[1].High);
        Assert.Equal(109, domainEntityPriceHistoryEntries[1].Low);
        Assert.Equal(110, domainEntityPriceHistoryEntries[1].Open);
        Assert.Equal(112, domainEntityPriceHistoryEntries[1].Close);
        Assert.Equal(111, domainEntityPriceHistoryEntries[1].Average, precision: 10);

        Assert.Single(domainEntity.Splits);
        Assert.Equal(new DateOnly(2020, 5, 20), domainEntity.Splits.First().Date);
        Assert.Equal(2.0, domainEntity.Splits.First().SplitRatio);

        Assert.Single(domainEntity.DistributionEvents);
        Assert.Equal(new DateOnly(2020, 5, 17), domainEntity.DistributionEvents[0].PayoutDate);
        Assert.Equal(0.4, domainEntity.DistributionEvents[0].PayoutAmount);
    }

    [Fact]
    public void Security_ToDomainEntity_CryptoCurrency()
    {
        Security extApiModel = new()
        {
            History = GetDailyPricesTestData_ForNonDividendPayingQuoteTypes(),
            Info = new()
            {
                QuoteType = QuoteType.Cryptocurrency,
                Symbol = "Symbol-USD",
                LongName = "Long Cryptocurrency Name",
                ShortName = "Cryptocurrency Name",
                FullDayPrice = 111.1,
                Currency = "USD",
                Exchange = "EXCHG",
                FullExchangeName = "EXCHANGE",
                MarketState = "POST",
                MarketCap = 9998887776
            }
        };

        var domainEntity = extApiModel.ToDomainEntity() as Domain.Securities.CryptoCurrency;

        Assert.NotNull(domainEntity);
        Assert.Equal(Domain.Securities.Base.SecurityType.Cryptocurrency, domainEntity.SecurityType);
        Assert.Equal(extApiModel.Info.Symbol, domainEntity.Symbol);
        Assert.Equal(extApiModel.Info.LongName, domainEntity.Name);
        Assert.Equal(extApiModel.Info.ShortName, domainEntity.ShortName);
        Assert.Equal(extApiModel.Info.MarketCap, domainEntity.MarketCapitalization);

        var domainEntityPriceHistoryEntries = domainEntity.PriceHistory.ToArray();
        Assert.Equal(2, domainEntityPriceHistoryEntries.Length);

        // Earlier date should come first/should be sorted
        Assert.Equal(new DateOnly(2020, 5, 17), domainEntityPriceHistoryEntries[0].Date);
        Assert.Equal(109, domainEntityPriceHistoryEntries[0].High);
        Assert.Equal(107, domainEntityPriceHistoryEntries[0].Low);
        Assert.Equal(108.1, domainEntityPriceHistoryEntries[0].Open);
        Assert.Equal(108.3, domainEntityPriceHistoryEntries[0].Close);
        Assert.Equal(108.2, domainEntityPriceHistoryEntries[0].Average, precision: 10);

        Assert.Equal(new DateOnly(2020, 5, 20), domainEntityPriceHistoryEntries[1].Date);
        Assert.Equal(114, domainEntityPriceHistoryEntries[1].High);
        Assert.Equal(109, domainEntityPriceHistoryEntries[1].Low);
        Assert.Equal(110, domainEntityPriceHistoryEntries[1].Open);
        Assert.Equal(112, domainEntityPriceHistoryEntries[1].Close);
        Assert.Equal(111, domainEntityPriceHistoryEntries[1].Average, precision: 10);
    }

    [Fact]
    public void Security_ToDomainEntity_CurrencyPair()
    {
        Security extApiModel = new()
        {
            History = GetDailyPricesTestData_ForNonDividendPayingQuoteTypes(),
            Info = new()
            {
                QuoteType = QuoteType.CurrencyPair,
                Symbol = "EUR-USD",
                LongName = "Euro - United States Dollar",
                ShortName = "Euro - USD",
                FullDayPrice = 1.11,
                Currency = "USD",
                Exchange = "EXCHG",
                FullExchangeName = "EXCHANGE",
                MarketState = "POST",
            }
        };

        var domainEntity = extApiModel.ToDomainEntity() as Domain.Securities.CurrencyPair;

        Assert.NotNull(domainEntity);
        Assert.Equal(Domain.Securities.Base.SecurityType.CurrencyPair, domainEntity.SecurityType);
        Assert.Equal(extApiModel.Info.Symbol, domainEntity.Symbol);
        Assert.Equal(extApiModel.Info.LongName, domainEntity.Name);
        Assert.Equal(extApiModel.Info.ShortName, domainEntity.ShortName);

        var domainEntityPriceHistoryEntries = domainEntity.PriceHistory.ToArray();
        Assert.Equal(2, domainEntityPriceHistoryEntries.Length);

        // Earlier date should come first/should be sorted
        Assert.Equal(new DateOnly(2020, 5, 17), domainEntityPriceHistoryEntries[0].Date);
        Assert.Equal(109, domainEntityPriceHistoryEntries[0].High);
        Assert.Equal(107, domainEntityPriceHistoryEntries[0].Low);
        Assert.Equal(108.1, domainEntityPriceHistoryEntries[0].Open);
        Assert.Equal(108.3, domainEntityPriceHistoryEntries[0].Close);
        Assert.Equal(108.2, domainEntityPriceHistoryEntries[0].Average, precision: 10);

        Assert.Equal(new DateOnly(2020, 5, 20), domainEntityPriceHistoryEntries[1].Date);
        Assert.Equal(114, domainEntityPriceHistoryEntries[1].High);
        Assert.Equal(109, domainEntityPriceHistoryEntries[1].Low);
        Assert.Equal(110, domainEntityPriceHistoryEntries[1].Open);
        Assert.Equal(112, domainEntityPriceHistoryEntries[1].Close);
        Assert.Equal(111, domainEntityPriceHistoryEntries[1].Average, precision: 10);
    }

    /// <summary>
    /// Purposefully provides incorrectly ordered entries to test ordering
    /// </summary>
    /// <returns></returns>
    private List<DailyPrice> GetDailyPricesTestData_ForDividendPayingQuoteTypes()
    {
        DailyPrice[] testData = [
                new() {
                    Date = new DateOnly(2020, 5, 20),
                    High = 114,
                    Low = 109,
                    Open = 110,
                    Close = 112,
                    Dividends = 0.0,
                    StockSplits = 2.0
                },
                new() {
                    Date = new DateOnly(2020, 5, 17),
                    High = 109,
                    Low = 107,
                    Open = 108.1,
                    Close = 108.3,
                    Dividends = 0.4,
                    StockSplits = 0
                }
            ];

        return testData.ToList();
    }

    /// <summary>
    /// Purposefully provides incorrectly ordered entries to test ordering
    /// </summary>
    /// <returns></returns>
    private List<DailyPrice> GetDailyPricesTestData_ForNonDividendPayingQuoteTypes()
    {
        DailyPrice[] testData = [
                new() {
                    Date = new DateOnly(2020, 5, 20),
                    High = 114,
                    Low = 109,
                    Open = 110,
                    Close = 112,
                },
                new() {
                    Date = new DateOnly(2020, 5, 17),
                    High = 109,
                    Low = 107,
                    Open = 108.1,
                    Close = 108.3,
                }
            ];

        return testData.ToList();
    }
}