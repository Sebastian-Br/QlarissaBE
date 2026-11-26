using System.Data;
using Microsoft.EntityFrameworkCore;
using Qlarissa.Application.Repositories;
using Qlarissa.Infrastructure.DB.Entities;

namespace Qlarissa.Infrastructure.DB.Repositories;

public sealed class RefreshTokenRepository(ApplicationDbContext context) : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task AddAsync(string userId, string tokenHash, DateTime createdAtUtc, DateTime expiresAtUtc)
    {
        _context.Set<RefreshTokenSession>().Add(new RefreshTokenSession
        {
            UserId = userId,
            TokenHash = tokenHash,
            CreatedAtUtc = createdAtUtc,
            ExpiresAtUtc = expiresAtUtc
        });
        await _context.SaveChangesAsync();
    }

    public async Task<string?> RotateAsync(
        string tokenHash,
        string replacementTokenHash,
        DateTime nowUtc,
        DateTime replacementExpiresAtUtc)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var current = await _context.Set<RefreshTokenSession>()
            .SingleOrDefaultAsync(session => session.TokenHash == tokenHash);
        if (current is null)
        {
            await transaction.CommitAsync();
            return null;
        }

        if (current.RevokedAtUtc.HasValue)
        {
            var activeSessions = await _context.Set<RefreshTokenSession>()
                .Where(session => session.UserId == current.UserId && session.RevokedAtUtc == null)
                .ToListAsync();
            foreach (var session in activeSessions)
                session.RevokedAtUtc = nowUtc;
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return null;
        }

        if (current.ExpiresAtUtc <= nowUtc)
        {
            current.RevokedAtUtc = nowUtc;
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return null;
        }

        current.RevokedAtUtc = nowUtc;
        current.ReplacedByTokenHash = replacementTokenHash;
        _context.Set<RefreshTokenSession>().Add(new RefreshTokenSession
        {
            UserId = current.UserId,
            TokenHash = replacementTokenHash,
            CreatedAtUtc = nowUtc,
            ExpiresAtUtc = replacementExpiresAtUtc
        });

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
        return current.UserId;
    }

    public async Task RevokeAsync(string tokenHash, DateTime nowUtc)
    {
        var session = await _context.Set<RefreshTokenSession>()
            .SingleOrDefaultAsync(item => item.TokenHash == tokenHash);
        if (session is null || session.RevokedAtUtc.HasValue)
            return;

        session.RevokedAtUtc = nowUtc;
        await _context.SaveChangesAsync();
    }
}
