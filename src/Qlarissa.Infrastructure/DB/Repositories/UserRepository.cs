using Microsoft.AspNetCore.Identity;
using Qlarissa.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Qlarissa.Domain;

namespace Qlarissa.Infrastructure.DB.Repositories;

public sealed class UserRepository(UserManager<QlarissaUser> userManager, ApplicationDbContext context, WatchlistRepository watchlistRepository) : IUserRepository
{
    readonly UserManager<QlarissaUser> _identityUserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    readonly WatchlistRepository _watchlistRepository = watchlistRepository ?? throw new ArgumentNullException(nameof(watchlistRepository));

    public async Task<IdentityResult> CreateAsync(QlarissaUser user, string password)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var result = await _identityUserManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                await transaction.RollbackAsync();
                return result;
            }

            await _watchlistRepository.AddWatchlistAsync(new Domain.Watchlists.Watchlist()
            {
                Name = "Primary",
                IsPinned = true,
                PinPosition = 1,
                TrackedChangeDurationTradingDays = 1
            }, user.Id);

            await _watchlistRepository.AddWatchlistAsync(new Domain.Watchlists.Watchlist()
            {
                Name = "Secondary",
                IsPinned = true,
                PinPosition = 2,
                TrackedChangeDurationTradingDays = 5
            }, user.Id);

            await transaction.CommitAsync();
            return result;

        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<QlarissaUser?> GetAsync(string username)
    {
        return await _identityUserManager.FindByNameAsync(username);
    }

    public Task<bool> CheckPasswordAsync(QlarissaUser user, string password)
    {
        return _identityUserManager.CheckPasswordAsync(user, password);
    }

    public Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken)
    {
        var normalizedName = _identityUserManager.NormalizeName(username);
        return _identityUserManager.Users.AnyAsync(u => u.NormalizedUserName == normalizedName, cancellationToken);
    }

    public Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken)
    {
        var normalizedEmail = _identityUserManager.NormalizeEmail(email);
        return _identityUserManager.Users.AnyAsync(u => u.NormalizedEmail == normalizedEmail, cancellationToken);
    }
}