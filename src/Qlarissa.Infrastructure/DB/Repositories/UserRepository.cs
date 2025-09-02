using Microsoft.AspNetCore.Identity;
using Qlarissa.Domain.Entities;
using Qlarissa.Infrastructure.DB.Repositories.Interfaces;

namespace Qlarissa.Infrastructure.DB.Repositories;

public sealed class UserRepository(UserManager<QlarissaUser> userManager) : IUserRepository
{
    readonly UserManager<QlarissaUser> _identityUserManager = userManager;

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
}