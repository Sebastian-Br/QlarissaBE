using Microsoft.AspNetCore.Identity;
using Qlarissa.Domain.Entities;

namespace Qlarissa.Application.Interfaces.Repositories;

public interface IUserRepository
{
    /// <summary>
    /// Creates a new user with the specified password.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="password">The password.</param>
    /// <returns>.Succeeded is true on success.</returns>
    Task<IdentityResult> CreateAsync(QlarissaUser user, string password);

    /// <summary>
    /// Retrieves a user by name.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <returns>The user, or null if not found.</returns>
    Task<QlarissaUser?> GetAsync(string username);

    /// <summary>
    /// Checks if the specified password is correct for the given user.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="password">The password.</param>
    /// <returns>true if the password is correct; otherwise, false.</returns>
    Task<bool> CheckPasswordAsync(QlarissaUser user, string password);
}