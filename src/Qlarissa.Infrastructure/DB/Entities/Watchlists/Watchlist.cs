using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Qlarissa.Infrastructure.DB.Entities.Watchlists;

public sealed class Watchlist
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public bool IsPinned { get; set; }

    public int PinPosition { get; set; }

    public int TrackedChangeDurationTradingDays { get; set; } = 1;

    public ICollection<WatchedSecurity> WatchedSecurities { get; set; } = [];

    // only used to create the Watchlist in the DB. WatchedSecurities are added later on.
    public static Watchlist FromDomainEntityWithoutCollections(Domain.Watchlists.Watchlist domainWatchlist, string userId)
    {
        return new Watchlist
        {
            Id = domainWatchlist.Id,
            UserId = userId,
            Name = domainWatchlist.Name,
            IsPinned = domainWatchlist.IsPinned,
            PinPosition = domainWatchlist.PinPosition,
            TrackedChangeDurationTradingDays = domainWatchlist.TrackedChangeDurationTradingDays
        };
    }

    // does not map WatchedSecurities.
    public Domain.Watchlists.Watchlist ToDomainEntityWithoutCollections()
    {
        return new Domain.Watchlists.Watchlist
        {
            Id = Id,
            Name = Name,
            IsPinned = IsPinned,
            PinPosition = PinPosition,
            TrackedChangeDurationTradingDays = TrackedChangeDurationTradingDays,
        };
    }
}

public class WatchlistConfiguration : IEntityTypeConfiguration<Watchlist>
{
    public void Configure(EntityTypeBuilder<Watchlist> builder)
    {
        builder.Property(s => s.Id).ValueGeneratedOnAdd();

        // Create index for querying by Id x UserId
        builder.HasIndex(w => new
        {
            w.Id,
            w.UserId
        });

        // For querying by UserId only (for GetAllWatchlistsAsync)
        builder.HasIndex(w => new
        {
            w.UserId
        });

        // Deletion of a user account should cascade to their watchlists
        builder.HasOne<Domain.QlarissaUser>()
            .WithMany()
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}