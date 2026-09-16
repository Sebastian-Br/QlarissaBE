using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Qlarissa.Infrastructure.DB.Entities.Base;

namespace Qlarissa.Infrastructure.DB.Entities;

public sealed class WatchedSecurity
{
    public string WatchedByUserId { get; set; } = string.Empty;

    public int SecurityId { get; set; }

    public PubliclyTradedSecurityBase Security { get; set; }

    public bool IsPrimaryWatchlist { get; set; }
}

public class WatchedSecurityConfiguration : IEntityTypeConfiguration<WatchedSecurity>
{
    public void Configure(EntityTypeBuilder<WatchedSecurity> builder)
    {
        // Prevent duplicate entries for the same user and security combination
        builder.HasKey(w => new
        {
            w.WatchedByUserId,
            w.SecurityId
        });

        builder.HasOne(w => w.Security)
               .WithMany()
               .HasForeignKey(w => w.SecurityId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Domain.QlarissaUser>()
            .WithMany()
            .HasForeignKey(w => w.WatchedByUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}