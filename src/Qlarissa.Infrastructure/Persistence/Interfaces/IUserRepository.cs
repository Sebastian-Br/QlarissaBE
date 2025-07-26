using Microsoft.AspNetCore.Identity;
using Qlarissa.Domain.Entities;

namespace Qlarissa.Infrastructure.Persistence.Interfaces;

public interface IUserRepository
{
    public Task<IdentityResult> CreateAsync(QlarissaUser user, string password);
}