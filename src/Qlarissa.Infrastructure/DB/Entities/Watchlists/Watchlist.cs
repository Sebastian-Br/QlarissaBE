using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Qlarissa.Infrastructure.DB.Entities.Watchlists;

public sealed class Watchlist
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public bool IsPrimary { get; set; }

    public bool IsSecondary { get; set; }

    public bool IsPinned { get; set; }

    public int PinPosition { get; set; }

    public int TrackedChangeDurationTradingDays { get; set; } = 1;

    public ICollection<WatchedSecurity> WatchedSecurities { get; set; } = [];

    // only used to create the Watchlist in the DB. WatchedSecurities are added later on.
    public static Watchlist FromDomainEntityWithoutCollections(Domain.Watchlists.WatchlistMinimal domainWatchlist, string userId)
    {
        return new Watchlist
        {
            Id = domainWatchlist.Id,
            UserId = userId,
            Name = domainWatchlist.Name,
            IsPrimary = domainWatchlist.IsPrimary,
            IsSecondary = domainWatchlist.IsSecondary,
            IsPinned = domainWatchlist.IsPinned,
            PinPosition = domainWatchlist.PinPosition,
            TrackedChangeDurationTradingDays = domainWatchlist.TrackedChangeDurationTradingDays
        };
    }

    // does not map WatchedSecurities.
    public Domain.Watchlists.WatchlistMinimal ToDomainEntity()
    {
        return new Domain.Watchlists.WatchlistMinimal
        {
            Id = Id,
            Name = Name,
            IsPrimary = IsPrimary,
            IsSecondary = IsSecondary,
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
        }).IsUnique();

        // For querying by UserId only (for GetAllWatchlistsAsync)
        builder.HasIndex(w => new
        {
            w.UserId
        });

        builder.HasIndex(
            w => w.UserId,
            "IX_Watchlists_UserId_Primary")
            .IsUnique()
            .HasFilter("[IsPrimary] = 1");

        builder.HasIndex(
            w => w.UserId,
            "IX_Watchlists_UserId_Secondary")
            .IsUnique()
            .HasFilter("[IsSecondary] = 1");

        // Deletion of a user account should cascade to their watchlists
        builder.HasOne<Domain.QlarissaUser>()
            .WithMany()
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}