using Qlarissa.Domain.Entities.Securities;
using Qlarissa.Domain.Entities.Securities.Base;
using Qlarissa.Domain.Entities.Securities.MarketData;

namespace Qlarissa.Infrastructure.PyFinance;

public class Security
{
    public SecurityInformation Info { get; set; }

    public IEnumerable<DailyPrice> History { get; set; }

    /// <summary>
    /// Set for some ETFs/Stocks, usually US ones. Not set for Cryptocurrencies or CurrencyPairs.
    /// </summary>
    public string ISIN {  get; set; }

    public PubliclyTradedSecurityBase ToDomainEntity()
    {
        PubliclyTradedSecurityBase domainEntity;

        if (Info.QuoteType == QuoteType.Stock)
        {
            var stock = new Stock
            {
                SecurityType = SecurityType.Stock,
                ISIN = ISIN,
                InvestorRelationsURL = Info.IrWebsite,
                BusinessSummary = Info.LongBusinessSummary,
                MarketCapitalization = Info.MarketCap,
                DividendRate = Info.DividendRate,
                TargetMeanPrice = Info.TargetMeanPrice,
                RecommendationMean = Info.RecommendationMean,
                DividendPayouts = History.Where(h => h.Dividends > 0).Select(x => new DividendPayout
                {
                    PayoutDate = x.Date,
                    PayoutAmount = x.Dividends
                }).OrderBy(x => x.PayoutDate).ToList(),
                Splits = History.Where(h => h.StockSplits > 0).Select(x => new Split
                {
                    Date = x.Date,
                    SplitRatio = x.StockSplits
                }).OrderBy(x => x.Date).ToList()
            };
            domainEntity = stock;
        } else if (Info.QuoteType == QuoteType.ETF)
        {
            var etf = new ETF
            {
                SecurityType = SecurityType.ETF,
                ISIN = ISIN,
                NetExpenseRatio = Info.NetExpenseRatio,
                DividendYield = Info.DividendYield,
                DistributionEvents = History.Where(h => h.Dividends > 0).Select(x => new DividendPayout
                {
                    PayoutDate = x.Date,
                    PayoutAmount = x.Dividends
                }).OrderBy(x => x.PayoutDate).ToList(),
                Splits = History.Where(h => h.StockSplits > 0).Select(x => new Split
                {
                    Date = x.Date,
                    SplitRatio = x.StockSplits
                }).OrderBy(x => x.Date).ToList()
            };
            domainEntity = etf;
        } else if (Info.QuoteType == QuoteType.Cryptocurrency)
        {
            var cryptoCurrency = new CryptoCurrency
            {
                SecurityType = SecurityType.Cryptocurrency,
                MarketCapitalization = Info.MarketCap
            };
            domainEntity = cryptoCurrency;
        }
        else if (Info.QuoteType == QuoteType.CurrencyPair)
        {
            var currencyPair = new CurrencyPair
            {
                SecurityType = SecurityType.CurrencyPair
            };
            domainEntity = currencyPair;
        }
        else
        {
            throw new NotSupportedException($"QuoteType '{Info.QuoteType}' is not supported.");
        }

        domainEntity.Name = Info.LongName;
        domainEntity.ShortName = Info.ShortName;
        // domainEntity.Currency -- loaded outside of mapper
        domainEntity.ExchangeName = Info.FullExchangeName;
        domainEntity.ExchangeShortName = Info.Exchange;
        domainEntity.Symbol = Info.Symbol;
        domainEntity.PriceHistory = History.Select(h => h.ToDomainEntity()).ToList();
        domainEntity.Price = Info.FullDayPrice;
        domainEntity.PriceLastUpdatedTime = DateTime.UtcNow;
        domainEntity.LastCompleteUpdateTime = DateTime.UtcNow;

        return domainEntity;
    }
}