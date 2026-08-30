using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Qlarissa.Infrastructure.DB.Entities.MarketData;

namespace Qlarissa.Infrastructure.DB.Entities.Base;

public class PubliclyTradedSecurityBase : SecurityBase
{
    public string ExchangeName { get; set; }

    public string ExchangeShortName { get; set; }

    public string Symbol { get; set; }

    public double Price { get; set; }

    public DateTime PriceLastUpdatedTime { get; set; }

    public DateTime LastCompleteUpdateTime { get; set; }

    public ICollection<DailyPrice> PriceHistory { get; set; } = [];

    /// <summary>
    /// Dividend payouts for ETFs and Stocks. Conversion is handled in Stock/ETF entities.
    /// </summary>
    public ICollection<DividendPayout> DividendPayouts { get; set; } = [];

    /// <summary>
    /// Split events for Stocks/ETFs.Conversion is handled in Stock/ETF entities.
    /// </summary>
    public ICollection<Split> Splits { get; set; } = [];

    public static void FromDomainEntity(Domain.Entities.Securities.Base.PubliclyTradedSecurityBase domainEntity, PubliclyTradedSecurityBase dbEntity)
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
        dbEntity.LastCompleteUpdateTime = domainEntity.LastCompleteUpdateTime;
        dbEntity.PriceHistory = domainEntity.PriceHistory.Select(x => DailyPrice.FromDomainEntity(x, domainEntity)).ToList();
    }

    public static void ToDomainEntity(Domain.Entities.Securities.Base.PubliclyTradedSecurityBase domainEntity, PubliclyTradedSecurityBase dbEntity)
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
        domainEntity.LastCompleteUpdateTime = dbEntity.LastCompleteUpdateTime;
        domainEntity.PriceHistory = dbEntity.PriceHistory.Select(DailyPrice.ToDomainEntity).ToList();
    }
}

public class PubliclyTradedSecurityBaseConfiguration : IEntityTypeConfiguration<PubliclyTradedSecurityBase>
{
    public void Configure(EntityTypeBuilder<PubliclyTradedSecurityBase> builder)
    {
        builder.HasIndex(s => s.Symbol).IsUnique();
    }
}