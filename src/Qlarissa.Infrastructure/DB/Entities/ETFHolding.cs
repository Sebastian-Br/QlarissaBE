using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Qlarissa.Infrastructure.DB.Entities;

public sealed class ETFHolding
{
    public int Id { get; set; }

    public int ETFid { get; set; }

    public ETF ETF { get; set; }

    public int PortfolioId { get; set; }

    public int Amount { get; set; }
}

public class ETFHoldingConfiguration : IEntityTypeConfiguration<ETFHolding>
{
    public void Configure(EntityTypeBuilder<ETFHolding> builder)
    {
        builder.Property(s => s.Id).ValueGeneratedOnAdd();

        builder.HasOne(sh => sh.ETF)
            .WithMany()
            .HasForeignKey(sh => sh.ETFid)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasOne<Portfolio>()
            .WithMany(s => s.ETFHoldings)
            .HasForeignKey(dp => dp.PortfolioId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasIndex(s => s.PortfolioId);
    }
}