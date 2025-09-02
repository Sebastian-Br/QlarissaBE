using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Qlarissa.Infrastructure.DB.Entities;

public sealed class StockHolding
{
    public int Id { get; set; }

    public int StockId { get; set; }
    public Stock Stock { get; set; }

    public int PortfolioId { get; set; }

    public int Amount { get; set; }
}

public class StockHoldingConfiguration : IEntityTypeConfiguration<StockHolding>
{
    public void Configure(EntityTypeBuilder<StockHolding> builder)
    {
        builder.Property(s => s.Id).ValueGeneratedOnAdd();

        builder.HasOne(sh => sh.Stock)
            .WithMany()
            .HasForeignKey(sh => sh.StockId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasOne<Portfolio>()
            .WithMany(s => s.StockHoldings)
            .HasForeignKey(dp => dp.PortfolioId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasIndex(s => s.PortfolioId);
    }
}