using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Qlarissa.Infrastructure.DB.Entities.MarketData;

namespace Qlarissa.Infrastructure.DB.Entities.Base;

public class PubliclyTradedSecurityBase : SecurityBase
{
    public string Symbol { get; set; }

    public decimal Price { get; set; }

    public ICollection<DailyPrice> PriceHistory { get; set; } = [];

    public DividendPayout[] DividendPayouts { get; set; }
}

public class PubliclyTradedSecurityBaseConfiguration : IEntityTypeConfiguration<PubliclyTradedSecurityBase>
{
    public void Configure(EntityTypeBuilder<PubliclyTradedSecurityBase> builder)
    {
        builder.HasIndex(s => s.Symbol).IsUnique();
    }
}