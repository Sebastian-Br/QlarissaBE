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

    public ICollection<DailyPrice> PriceHistory { get; set; } = [];

    /// <summary>
    /// Dividend payouts for ETFs and Stocks. Conversion is handled in Stock/ETF entities.
    /// </summary>
    public ICollection<DividendPayout> DividendPayouts { get; set; } = [];

    /// <summary>
    /// Split events for Stocks/ETFs.Conversion is handled in Stock/ETF entities.
    /// </summary>
    public ICollection<Split> Splits { get; set; } = [];

    protected static void FromDomainEntity(Domain.Entities.Securities.Base.PubliclyTradedSecurityBase domainEntity, PubliclyTradedSecurityBase dbEntity)
    {
        dbEntity.Id = domainEntity.Id;
        dbEntity.Name = domainEntity.Name;
        dbEntity.CurrencyId = domainEntity.Currency.Id; // When adding a security, the currency must already exist in the database.
        dbEntity.SecurityType = (SecurityType)domainEntity.SecurityType;
        dbEntity.ExchangeName = domainEntity.ExchangeName;
        dbEntity.ExchangeShortName = domainEntity.ExchangeShortName;
        dbEntity.Symbol = domainEntity.Symbol;
        dbEntity.Price = domainEntity.Price;
        dbEntity.PriceLastUpdatedTime = domainEntity.PriceLastUpdatedTime;
        dbEntity.PriceHistoryLastDataPointDate = domainEntity.PriceHistoryLastDataPointDate;
        dbEntity.PriceHistory = domainEntity.PriceHistory.Select(x => DailyPrice.FromDomainEntity(x, domainEntity)).ToList();
    }

    public static PubliclyTradedSecurityBase FromDomainEntity(Domain.Entities.Securities.Base.PubliclyTradedSecurityBase domainEntity)
    {
        return domainEntity switch
        {
            Domain.Entities.Securities.Stock stock => Stock.FromDomainEntity(stock),
            Domain.Entities.Securities.ETF etf => ETF.FromDomainEntity(etf),
            Domain.Entities.Securities.CryptoCurrency cryptoCurrency => CryptoCurrency.FromDomainEntity(cryptoCurrency),
            Domain.Entities.Securities.CurrencyPair currencyPair => CurrencyPair.FromDomainEntity(currencyPair),
            _ => throw new NotImplementedException($"Unsupported security type '{domainEntity.GetType().Name}'.")
        };
    }

    protected static void ToDomainEntity(Domain.Entities.Securities.Base.PubliclyTradedSecurityBase domainEntity, PubliclyTradedSecurityBase dbEntity)
    {
        domainEntity.Id = dbEntity.Id;
        domainEntity.Name = dbEntity.Name;
        domainEntity.Currency = dbEntity.Currency.ToDomainEntity();
        domainEntity.SecurityType = (Domain.Entities.Securities.Base.SecurityType)dbEntity.SecurityType;
        domainEntity.ExchangeName = dbEntity.ExchangeName;
        domainEntity.ExchangeShortName= dbEntity.ExchangeShortName;
        domainEntity.Symbol = dbEntity.Symbol;
        domainEntity.Price = dbEntity.Price;
        domainEntity.PriceLastUpdatedTime = dbEntity.PriceLastUpdatedTime;
        domainEntity.PriceHistoryLastDataPointDate = dbEntity.PriceHistoryLastDataPointDate;
        domainEntity.PriceHistory = dbEntity.PriceHistory.Select(DailyPrice.ToDomainEntity).ToList();
    }

    public Domain.Entities.Securities.Base.PubliclyTradedSecurityBase ToDomainEntity()
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

    public void UpdateFromDomainEntity(Domain.Entities.Securities.Base.PubliclyTradedSecurityBase domainEntity)
    {
        var incomingDbEntity = FromDomainEntity(domainEntity);
        Name = incomingDbEntity.Name;
        ExchangeName = incomingDbEntity.ExchangeName;
        ExchangeShortName = incomingDbEntity.ExchangeShortName;
        Symbol = incomingDbEntity.Symbol;
        Price = incomingDbEntity.Price;
        PriceLastUpdatedTime = DateTime.UtcNow;
        PriceHistoryLastDataPointDate = incomingDbEntity.PriceHistory.Last().Date;
    }
}

public class PubliclyTradedSecurityBaseConfiguration : IEntityTypeConfiguration<PubliclyTradedSecurityBase>
{
    public void Configure(EntityTypeBuilder<PubliclyTradedSecurityBase> builder)
    {
        builder.HasIndex(s => s.Symbol).IsUnique();
    }
}