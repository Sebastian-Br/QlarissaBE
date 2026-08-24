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
    /// <param name="username"></param>
    /// <returns>The user, or null if none is found.</returns>
    Task<QlarissaUser?> GetAsync(string username);

    /// <summary>
    /// Checks if the specified password is correct for the given user.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="password"></param>
    /// <returns>True if the password is correct. False otherwise.</returns>
    Task<bool> CheckPasswordAsync(QlarissaUser user, string password);

    /// <summary>
    /// Checks if a username already exists in the system.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>True if it exists. False otherwise.</returns>
    Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken);

    /// <summary>
    /// Checks if an email already exists in the system.
    /// </summary>
    /// <param name="email"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>True if it exists. False otherwise.</returns>
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken);
}