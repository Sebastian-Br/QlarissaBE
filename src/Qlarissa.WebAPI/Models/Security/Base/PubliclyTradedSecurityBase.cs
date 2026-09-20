using Qlarissa.WebAPI.Models.Security.MarketData;

namespace Qlarissa.WebAPI.Models.Security.Base;

public abstract class PubliclyTradedSecurityBase : SecurityBase
{
    public string ExchangeName { get; set; } = string.Empty;

    public string ExchangeShortName { get; set; } = string.Empty;

    public string Symbol { get; set; } = string.Empty;

    public IReadOnlyList<DailyPrice> PriceHistory { get; set; } = [];

    public double Price { get; set; }

    public DateTime PriceLastUpdatedTime { get; set; }

    public DateTime LastCompleteUpdateTime { get; set; }

    protected static void FromDomainEntity(Domain.Securities.Base.PubliclyTradedSecurityBase domainEntity, PubliclyTradedSecurityBase webApiModel)
    {
        SecurityBase.FromDomainEntity(domainEntity, webApiModel);
        webApiModel.ExchangeName = domainEntity.ExchangeName;
        webApiModel.ExchangeShortName = domainEntity.ExchangeShortName;
        webApiModel.Symbol = domainEntity.Symbol;
        webApiModel.Price = domainEntity.Price;
        webApiModel.PriceLastUpdatedTime = domainEntity.PriceLastUpdatedTime;
        webApiModel.LastCompleteUpdateTime = domainEntity.InformationLastUpdatedTime;
        webApiModel.PriceHistory = domainEntity.PriceHistory.Select(DailyPrice.FromDomainEntity).ToList();
    }

    public static PubliclyTradedSecurityBase FromDomainEntity(Domain.Securities.Base.PubliclyTradedSecurityBase domainEntity)
    {
        return domainEntity switch
        {
            Domain.Securities.Stock stock => Stock.FromDomainEntity(stock),
            Domain.Securities.ETF etf => ETF.FromDomainEntity(etf),
            Domain.Securities.CryptoCurrency cryptoCurrency => CryptoCurrency.FromDomainEntity(cryptoCurrency),
            Domain.Securities.CurrencyPair currencyPair => CurrencyPair.FromDomainEntity(currencyPair),
            _ => throw new NotImplementedException($"Unsupported security type '{domainEntity.GetType().Name}'.")
        };
    }
}