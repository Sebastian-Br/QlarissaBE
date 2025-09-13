using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Qlarissa.Infrastructure.DB.Entities.Base;

public class SecurityBase
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int? CurrencyId { get; set; }

    public Currency? Currency { get; set; }
}

public class SecurityBaseConfiguration : IEntityTypeConfiguration<SecurityBase>
{
    public void Configure(EntityTypeBuilder<SecurityBase> builder)
    {
        builder.Property(s => s.Id).ValueGeneratedOnAdd();

        builder.HasOne(s => s.Currency)
            .WithMany()
            .HasForeignKey(p => p.CurrencyId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}