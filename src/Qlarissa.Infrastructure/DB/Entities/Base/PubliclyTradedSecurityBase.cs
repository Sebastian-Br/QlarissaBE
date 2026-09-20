using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Qlarissa.Infrastructure.DB.Entities.MarketData;

namespace Qlarissa.Infrastructure.DB.Entities.Base;

public abstract class PubliclyTradedSecurityBase : SecurityBase
{
    public string ExchangeName { get; set; } = string.Empty;

    public string ExchangeShortName { get; set; } = string.Empty;

    public string Symbol { get; set; } = string.Empty;

    public double Price { get; set; }

    public DateTime PriceLastUpdatedTime { get; set; }

    /// <summary>
    /// The date of the last price data point in the PriceHistory collection. This is used to determine if the price history is up to date.
    /// It is different from PriceLastUpdatedTime, which may be updated more frequently.
    /// </summary>
    public DateOnly PriceHistoryLastDataPointDate { get; set; }

    public DateTime InformationLastUpdatedTime { get; set; }

    public ICollection<DailyPrice> PriceHistory { get; set; } = [];

    /// <summary>
    /// Dividend payouts for ETFs and Stocks. Conversion is handled in Stock/ETF entities.
    /// </summary>
    public ICollection<DividendPayout> DividendPayouts { get; set; } = [];

    /// <summary>
    /// Split events for Stocks/ETFs.Conversion is handled in Stock/ETF entities.
    /// </summary>
    public ICollection<Split> Splits { get; set; } = [];

    protected static void FromDomainEntity(Domain.Securities.Base.PubliclyTradedSecurityBase domainEntity, PubliclyTradedSecurityBase dbEntity)
    {
        SecurityBase.FromDomainEntity(domainEntity, dbEntity);
        dbEntity.ExchangeName = domainEntity.ExchangeName;
        dbEntity.ExchangeShortName = domainEntity.ExchangeShortName;
        dbEntity.Symbol = domainEntity.Symbol;
        dbEntity.Price = domainEntity.Price;
        dbEntity.PriceLastUpdatedTime = domainEntity.PriceLastUpdatedTime;
        dbEntity.PriceHistoryLastDataPointDate = domainEntity.PriceHistoryLastDataPointDate;
        dbEntity.InformationLastUpdatedTime = domainEntity.InformationLastUpdatedTime;
        dbEntity.PriceHistory = domainEntity.PriceHistory.Select(x => DailyPrice.FromDomainEntity(x, domainEntity)).ToList();
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

    protected static void ToDomainEntity(Domain.Securities.Base.PubliclyTradedSecurityBase domainEntity, PubliclyTradedSecurityBase dbEntity)
    {
        SecurityBase.ToDomainEntity(domainEntity, dbEntity);
        domainEntity.ExchangeName = dbEntity.ExchangeName;
        domainEntity.ExchangeShortName= dbEntity.ExchangeShortName;
        domainEntity.Symbol = dbEntity.Symbol;
        domainEntity.Price = dbEntity.Price;
        domainEntity.PriceLastUpdatedTime = dbEntity.PriceLastUpdatedTime;
        domainEntity.PriceHistoryLastDataPointDate = dbEntity.PriceHistoryLastDataPointDate;
        domainEntity.InformationLastUpdatedTime = dbEntity.InformationLastUpdatedTime;
        domainEntity.PriceHistory = dbEntity.PriceHistory.Select(DailyPrice.ToDomainEntity).ToList();
    }

    public Domain.Securities.Base.PubliclyTradedSecurityBase ToDomainEntity()
    {
        return this switch
        {
            Stock stock => stock.ToDomainEntity(),
            ETF etf => etf.ToDomainEntity(),
            CryptoCurrency cryptoCurrency => cryptoCurrency.ToDomainEntity(),
            CurrencyPair currencyPair => currencyPair.ToDomainEntity(),
            _ => throw new NotImplementedException($"Unsupported security type '{this.GetType().Name}'.")
        };
    }

    internal void UpdateFromDomainEntity(Domain.Securities.Base.PubliclyTradedSecurityBase domainEntity)
    {
        var incomingDbEntity = FromDomainEntity(domainEntity);
        Name = incomingDbEntity.Name;
        ExchangeName = incomingDbEntity.ExchangeName;
        ExchangeShortName = incomingDbEntity.ExchangeShortName;
        Symbol = incomingDbEntity.Symbol;

        switch (this)
        {
            case Stock stock:
                stock.UpdateFromDbEntity((Stock)incomingDbEntity);
                break;
            case ETF etf:
                etf.UpdateFromDbEntity((ETF)incomingDbEntity);
                break;
            case CryptoCurrency cryptoCurrency:
                cryptoCurrency.UpdateFromDbEntity((CryptoCurrency)incomingDbEntity);
                break;
            case CurrencyPair currencyPair:
                currencyPair.UpdateFromDbEntity((CurrencyPair)incomingDbEntity);
                break;
        }

        Price = incomingDbEntity.Price;
        var now = DateTime.UtcNow;
        PriceLastUpdatedTime = now;
        InformationLastUpdatedTime = now;
        if (incomingDbEntity.PriceHistory.Any())
        {
            PriceHistoryLastDataPointDate = incomingDbEntity.PriceHistory.Last().Date;
        }
    }
}

public class PubliclyTradedSecurityBaseConfiguration : IEntityTypeConfiguration<PubliclyTradedSecurityBase>
{
    public void Configure(EntityTypeBuilder<PubliclyTradedSecurityBase> builder)
    {
        builder.HasIndex(s => s.Symbol).IsUnique();
    }
}