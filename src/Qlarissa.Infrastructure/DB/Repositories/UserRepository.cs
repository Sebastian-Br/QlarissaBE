using Microsoft.AspNetCore.Identity;
using Qlarissa.Domain.Entities;
using Qlarissa.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Qlarissa.Infrastructure.DB.Repositories;

public sealed class UserRepository(UserManager<QlarissaUser> userManager) : IUserRepository
{
    readonly UserManager<QlarissaUser> _identityUserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

    public async Task<IdentityResult> CreateAsync(QlarissaUser user, string password)
    {
        return await _identityUserManager.CreateAsync(user, password);
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