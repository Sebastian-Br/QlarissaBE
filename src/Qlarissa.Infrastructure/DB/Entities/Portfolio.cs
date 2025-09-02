using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Qlarissa.Infrastructure.DB.Entities;

public sealed class Portfolio
{
    public int Id { get; set; }

    public ICollection<StockHolding> StockHoldings { get; set; } = [];

    public int AccountCurrencyId { get; set; }

    public Currency AccountCurrency { get; set; } = null!;
}

public class PortfolioConfiguration : IEntityTypeConfiguration<Portfolio>
{
    public void Configure(EntityTypeBuilder<Portfolio> builder)
    {
        builder.Property(s => s.Id).ValueGeneratedOnAdd();

        builder.HasOne(p => p.AccountCurrency)
            .WithMany()
            .HasForeignKey(p => p.AccountCurrencyId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}