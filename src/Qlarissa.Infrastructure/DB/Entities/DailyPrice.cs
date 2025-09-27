using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Qlarissa.Infrastructure.DB.Entities.Base;

namespace Qlarissa.Infrastructure.DB.Entities;

public sealed class DailyPrice
{
    public int Id { get; set; }
    public int SecurityId { get; set; }
    public PubliclyTradedSecurityBase Security { get; set; } = default!;
    public decimal Open { get; set; }
    public decimal Close { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Average { get; set; }
    public DateOnly Date { get; set; }

    public static DailyPrice FromDomainEntity(Domain.Entities.Securities.MarketData.DailyPrice domainEntity, Domain.Entities.Securities.Base.PubliclyTradedSecurityBase security) =>
        new()
        {
            Id = domainEntity.Id,
            SecurityId = security.Id,
            Open = domainEntity.Open,
            Close = domainEntity.Close,
            High = domainEntity.High,
            Low = domainEntity.Low,
            Average = domainEntity.Average,
            Date = domainEntity.Date
        };

    public static Domain.Entities.Securities.MarketData.DailyPrice ToDomainEntity(DailyPrice dbEntity) =>
        new()
        {
            Id = dbEntity.Id,
            Open = dbEntity.Open,
            Close = dbEntity.Close,
            High = dbEntity.High,
            Low = dbEntity.Low,
            Average = dbEntity.Average,
            Date = dbEntity.Date
        };
}

public class DailyPriceConfiguration : IEntityTypeConfiguration<DailyPrice>
{
    public void Configure(EntityTypeBuilder<DailyPrice> builder)
    {
        builder.Property(s => s.Id).ValueGeneratedOnAdd();

        builder.HasOne(dp => dp.Security)
            .WithMany(s => s.PriceHistory)
            .HasForeignKey(dp => dp.SecurityId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.Property(dp => dp.Date)
            .HasConversion(
                d => d.ToDateTime(TimeOnly.MinValue),
                d => DateOnly.FromDateTime(d));
    }
}