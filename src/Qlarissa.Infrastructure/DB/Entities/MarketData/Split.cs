using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Qlarissa.Infrastructure.DB.Entities.Base;

namespace Qlarissa.Infrastructure.DB.Entities.MarketData;

public sealed class Split
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    public double SplitRatio { get; set; }

    public int SecurityId { get; set; }

    public PubliclyTradedSecurityBase Security { get; set; }

    public static Split FromDomainEntity(Domain.Entities.Securities.MarketData.Split domainEntity, Domain.Entities.Securities.Base.PubliclyTradedSecurityBase security)
    {
        return new Split
        {
            Id = domainEntity.Id,
            Date = domainEntity.Date,
            SplitRatio = domainEntity.SplitRatio,
            SecurityId = security.Id
        };
    }

    public static Domain.Entities.Securities.MarketData.Split ToDomainEntity(Split split)
    {
        return new Domain.Entities.Securities.MarketData.Split
        {
            Id = split.Id,
            Date = split.Date,
            SplitRatio = split.SplitRatio
        };
    }
}

public class SplitConfiguration : IEntityTypeConfiguration<Split>
{
    public void Configure(EntityTypeBuilder<Split> builder)
    {
        builder.Property(s => s.Id).ValueGeneratedOnAdd();

        builder.Property(s => s.Date)
            .HasConversion(
                d => d.ToDateTime(TimeOnly.MinValue),
                d => DateOnly.FromDateTime(d));

        builder.HasOne(dp => dp.Security)
            .WithMany(security => security.Splits)
            .HasForeignKey(dp => dp.SecurityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}