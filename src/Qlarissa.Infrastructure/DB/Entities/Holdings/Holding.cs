using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Qlarissa.Infrastructure.DB.Entities.Base;

namespace Qlarissa.Infrastructure.DB.Entities.Holdings;

public class Holding
{
    public int Id { get; set; }
    public int PortfolioId { get; set; }
    public int SecurityId { get; set; }
    public PubliclyTradedSecurityBase Security { get; set; }
    public IEnumerable<HoldingEvent> HoldingEvents { get; set; } = [];
}

public class HoldingConfiguration : IEntityTypeConfiguration<Holding>
{
    public void Configure(EntityTypeBuilder<Holding> builder)
    {
        builder.Property(holding => holding.Id).ValueGeneratedOnAdd();

        builder.HasOne(holding => holding.Security)
            .WithMany()
            .HasForeignKey(holding => holding.SecurityId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasOne<Portfolio>()
            .WithMany(portfolio => portfolio.Holdings)
            .HasForeignKey(holding => holding.PortfolioId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasIndex(holding => holding.PortfolioId);

        builder.HasMany(holding => holding.HoldingEvents)
            .WithOne()
            .HasForeignKey(holdingEvent => holdingEvent.HoldingId)
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}