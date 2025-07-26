using Microsoft.AspNetCore.Identity;
using Qlarissa.Domain.Entities;
using Qlarissa.Infrastructure.Persistence.Interfaces;

namespace Qlarissa.Infrastructure.Persistence;

public sealed class UserRepository(UserManager<QlarissaUser> userManager) : IUserRepository
{
    readonly UserManager<QlarissaUser> _identityUserManager = userManager;

    public async Task<IdentityResult> CreateAsync(QlarissaUser user, string password)
    {
        return await _identityUserManager.CreateAsync(user, password);
    }
}