namespace Qlarissa.Infrastructure.PyFinance;

public class Security
{
    public SecurityInformation Info { get; set; }

    public List<DailyPrice> History { get; set; }

    /// <summary>
    /// Set for some ETFs/Stocks, usually US ones. Not set for Cryptocurrencies or CurrencyPairs.
    /// "-" is the default value the API returns when no ISIN is available.
    /// </summary>
    public string ISIN {  get; set; } = "-";

    public Domain.Securities.Base.PubliclyTradedSecurityBase ToDomainEntity()
    {
        Domain.Securities.Base.PubliclyTradedSecurityBase domainEntity;

        if (Info.QuoteType == QuoteType.Stock)
        {
            var stock = new Domain.Securities.Stock
            {
                SecurityType = Domain.Securities.Base.SecurityType.Stock,
                ISIN = ISIN,
                InvestorRelationsURL = Info.IrWebsite,
                BusinessSummary = Info.LongBusinessSummary,
                SharesOutstanding = Info.SharesOutstanding,
                DividendRate = Info.DividendRate,
                TargetMeanPrice = Info.TargetMeanPrice,
                RecommendationMean = Info.RecommendationMean,
                DividendPayouts = History.Where(h => h.Dividends > 0)
                .Select(x => new Domain.Securities.MarketData.DividendPayout
                {
                    PayoutDate = x.Date,
                    PayoutAmount = x.Dividends
                })
                .OrderBy(x => x.PayoutDate).ToList(),
                Splits = History.Where(h => h.StockSplits > 0)
                .Select(x => new Domain.Securities.MarketData.Split
                {
                    Date = x.Date,
                    SplitRatio = x.StockSplits
                })
                .OrderBy(x => x.Date).ToList()
            };
            domainEntity = stock;
        } else if (Info.QuoteType == QuoteType.ETF)
        {
            var etf = new Domain.Securities.ETF
            {
                SecurityType = Domain.Securities.Base.SecurityType.ETF,
                ISIN = ISIN,
                NetExpenseRatio = Info.NetExpenseRatio,
                DividendYield = Info.DividendYield,
                DistributionEvents = History
                .Where(h => h.Dividends > 0)
                .Select(x => new Domain.Securities.MarketData.DividendPayout
                {
                    PayoutDate = x.Date,
                    PayoutAmount = x.Dividends
                })
                .OrderBy(x => x.PayoutDate).ToList(),
                Splits = History.Where(h => h.StockSplits > 0)
                .Select(x => new Domain.Securities.MarketData.Split
                {
                    Date = x.Date,
                    SplitRatio = x.StockSplits
                })
                .OrderBy(x => x.Date).ToList()
            };
            domainEntity = etf;
        } else if (Info.QuoteType == QuoteType.Cryptocurrency)
        {
            var cryptoCurrency = new Domain.Securities.CryptoCurrency
            {
                SecurityType = Domain.Securities.Base.SecurityType.Cryptocurrency,
                MarketCapitalization = Info.MarketCap
            };
            domainEntity = cryptoCurrency;
        }
        else if (Info.QuoteType == QuoteType.CurrencyPair)
        {
            var currencyPair = new Domain.Securities.CurrencyPair
            {
                SecurityType = Domain.Securities.Base.SecurityType.CurrencyPair
            };
            domainEntity = currencyPair;
        }
        else
        {
            throw new NotSupportedException($"QuoteType '{Info.QuoteType}' is not supported.");
        }

        domainEntity.Name = Info.LongName;
        domainEntity.ShortName = Info.ShortName;
        domainEntity.Currency = new Domain.Currency { Symbol = Info.Currency }; // fully loaded later on in the application layer
        domainEntity.ExchangeName = Info.FullExchangeName;
        domainEntity.ExchangeShortName = Info.Exchange;
        domainEntity.Symbol = Info.Symbol;
        domainEntity.PriceHistory = History.Select(h => h.ToDomainEntity()).ToList();
        domainEntity.Price = Info.FullDayPrice;
        domainEntity.PriceLastUpdatedTime = DateTime.UtcNow;

        return domainEntity;
    }
}