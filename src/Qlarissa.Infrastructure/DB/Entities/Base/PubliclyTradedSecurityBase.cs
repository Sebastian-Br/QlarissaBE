using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Qlarissa.Infrastructure.DB.Entities.MarketData;

namespace Qlarissa.Infrastructure.DB.Entities.Base;

public class PubliclyTradedSecurityBase : SecurityBase
{
    public string Symbol { get; set; }

    public decimal Price { get; set; }

    public DateTime PriceLastUpdatedTime { get; set; }

    public DateTime LastCompleteUpdateTime { get; set; }

    public ICollection<DailyPrice> PriceHistory { get; set; } = [];

    /// <summary>
    /// Conversion handled in Stock/ETF EF classes.
    /// </summary>
    public ICollection<DividendPayout> DividendPayouts { get; set; } = [];

    public static void FromDomainEntity(Domain.Entities.Securities.Base.PubliclyTradedSecurityBase domainEntity, PubliclyTradedSecurityBase dbEntity)
    {
        dbEntity.Id = domainEntity.Id;
        dbEntity.Name = domainEntity.Name;
        dbEntity.CurrencyId = domainEntity.Currency.Id; // When adding a security, the currency must have been added before
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
        domainEntity.Symbol = dbEntity.Symbol;
        domainEntity.Price = dbEntity.Price;
        domainEntity.PriceLastUpdatedTime = dbEntity.PriceLastUpdatedTime;
        domainEntity.LastCompleteUpdateTime = dbEntity.LastCompleteUpdateTime;
        domainEntity.PriceHistory = dbEntity.PriceHistory.Select(DailyPrice.ToDomainEntity).ToArray();
    }
}

public class PubliclyTradedSecurityBaseConfiguration : IEntityTypeConfiguration<PubliclyTradedSecurityBase>
{
    public void Configure(EntityTypeBuilder<PubliclyTradedSecurityBase> builder)
    {
        builder.HasIndex(s => s.Symbol).IsUnique();
    }
}