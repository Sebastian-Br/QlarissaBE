using Microsoft.AspNetCore.Identity;
using Qlarissa.Domain.Entities;

namespace Qlarissa.Infrastructure.Persistence.Interfaces;

public interface IUserRepository
{
    Task<IdentityResult> CreateAsync(QlarissaUser user, string password);
    Task<QlarissaUser?> GetAsync(string username);
    Task<bool> CheckPasswordAsync(QlarissaUser user, string password);
}