using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Qlarissa.Infrastructure.DB.Entities.Base;

namespace Qlarissa.Infrastructure.DB.Entities.MarketData;

public sealed class DailyPrice
{
    public int Id { get; set; }
    public int SecurityId { get; set; }
    public PubliclyTradedSecurityBase Security { get; set; } = default!;
    public double Open { get; set; }
    public double Close { get; set; }
    public double High { get; set; }
    public double Low { get; set; }
    public double Average { get; set; }
    public DateOnly Date { get; set; }

    public static DailyPrice FromDomainEntity(Domain.Securities.MarketData.DailyPrice domainEntity, Domain.Securities.Base.PubliclyTradedSecurityBase security) =>
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

    public static Domain.Securities.MarketData.DailyPrice ToDomainEntity(DailyPrice dbEntity) =>
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

    public void UpdateFromDbEntity(DailyPrice dbEntity)
    {
        Open = dbEntity.Open;
        Close = dbEntity.Close;
        High = dbEntity.High;
        Low = dbEntity.Low;
        Average = dbEntity.Average;
        // date should always remain the same
    }
}

public class DailyPriceConfiguration : IEntityTypeConfiguration<DailyPrice>
{
    public void Configure(EntityTypeBuilder<DailyPrice> builder)
    {
        builder.Property(s => s.Id).ValueGeneratedOnAdd();

        // prevent duplicate entries for the same day and security
        builder.HasIndex(dp => new { dp.SecurityId, dp.Date })
            .IsUnique();

        builder.Property(dp => dp.Date)
            .HasConversion(
                d => d.ToDateTime(TimeOnly.MinValue),
                d => DateOnly.FromDateTime(d));

        // deleting a security should cascade to its price history
        builder.HasOne(dp => dp.Security)
            .WithMany(s => s.PriceHistory)
            .HasForeignKey(dp => dp.SecurityId)
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}