using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Qlarissa.Infrastructure.DB.Entities;

public sealed class RefreshTokenSession
{
    public long Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string TokenHash { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime ExpiresAtUtc { get; set; }
    public DateTime? RevokedAtUtc { get; set; }
    public string? ReplacedByTokenHash { get; set; }
}

public sealed class RefreshTokenSessionConfiguration : IEntityTypeConfiguration<RefreshTokenSession>
{
    public void Configure(EntityTypeBuilder<RefreshTokenSession> builder)
    {
        builder.ToTable("RefreshTokenSessions");
        builder.HasKey(session => session.Id);
        builder.Property(session => session.UserId).HasMaxLength(450).IsRequired();
        builder.Property(session => session.TokenHash).HasMaxLength(64).IsRequired();
        builder.Property(session => session.ReplacedByTokenHash).HasMaxLength(64);
        builder.HasIndex(session => session.TokenHash).IsUnique();
        builder.HasIndex(session => new { session.UserId, session.RevokedAtUtc });
    }
}
