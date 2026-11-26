namespace Qlarissa.Application.Repositories;

public interface IRefreshTokenRepository
{
    Task AddAsync(string userId, string tokenHash, DateTime createdAtUtc, DateTime expiresAtUtc);

    Task<string?> RotateAsync(
        string tokenHash,
        string replacementTokenHash,
        DateTime nowUtc,
        DateTime replacementExpiresAtUtc);

    Task RevokeAsync(string tokenHash, DateTime nowUtc);
}
