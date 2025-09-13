using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Qlarissa.Infrastructure.DB.Entities.Base;

namespace Qlarissa.Infrastructure.DB.Entities.MarketData;

public sealed class DividendPayout
{
    public int Id { get; set; }

    public DateOnly PayoutDate { get; set; }

    public decimal PayoutAmount { get; set; }

    public int SecurityId { get; set; }

    public PubliclyTradedSecurityBase Security { get; set; }

    public static DividendPayout FromDomainEntity(Domain.Entities.Securities.MarketData.DividendPayout payout, Domain.Entities.Securities.Base.PubliclyTradedSecurityBase security)
        => new()
        {
            PayoutDate = payout.PayoutDate,
            PayoutAmount = payout.PayoutAmount,
            SecurityId = security.Id
        };

    public static Domain.Entities.Securities.MarketData.DividendPayout ToDomainEntity(DividendPayout dbEntity)
        => new()
        {
            PayoutDate = dbEntity.PayoutDate,
            PayoutAmount = dbEntity.PayoutAmount,
        };
}

public class DividendPayoutConfiguration : IEntityTypeConfiguration<DividendPayout>
{
    public void Configure(EntityTypeBuilder<DividendPayout> builder)
    {
        builder.Property(s => s.Id).ValueGeneratedOnAdd();

        builder.Property(dp => dp.PayoutDate)
            .HasConversion(
                d => d.ToDateTime(TimeOnly.MinValue),
                d => DateOnly.FromDateTime(d));

        builder.HasOne(dp => dp.Security)
            .WithMany(s => s.DividendPayouts)
            .HasForeignKey(dp => dp.SecurityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}