using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Qlarissa.Infrastructure.DB.Entities.Base;

namespace Qlarissa.Infrastructure.DB.Entities.Watchlists;

public sealed class WatchedSecurity
{
    public int WatchlistId { get; set; }

    public int SecurityId { get; set; }

    // Used to navigate to the security Name, Symbol, and previous close price.
    public PubliclyTradedSecurityBase Security { get; set; }

    public static WatchedSecurity FromDomainEntity(Domain.Watchlists.Watchlist domainWatchlist, Domain.Watchlists.WatchedSecurity domainWatchedSecurity)
    {
        return new WatchedSecurity
        {
            WatchlistId = domainWatchlist.Id,
            SecurityId = domainWatchedSecurity.SecurityId
        };
    }
}

public class WatchedSecurityConfiguration : IEntityTypeConfiguration<WatchedSecurity>
{
    public void Configure(EntityTypeBuilder<WatchedSecurity> builder)
    {
        // Prevent duplicate entries for the same watchlist and security combination
        builder.HasKey(w => new
        {
            w.WatchlistId,
            w.SecurityId
        });

        builder.HasOne(w => w.Security)
               .WithMany()
               .HasForeignKey(w => w.SecurityId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Watchlist>()
            .WithMany(x => x.WatchedSecurities)
            .HasForeignKey(w => w.WatchlistId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}