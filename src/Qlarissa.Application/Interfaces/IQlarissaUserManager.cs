using FluentResults;
using Qlarissa.Domain.Entities;
using System.Security.Claims;

namespace Qlarissa.Application.Interfaces;

public interface IQlarissaUserManager
{
    Task<Result<string>> LoginAsync(string username, string password);

    Task<Result<QlarissaUser>> GetAsync(ClaimsPrincipal user);

    Task<Result> RegisterAsync(string username, string email, string password);

    /// <summary>
    /// Checks if the given username exists in the system.
    /// </summary>
    /// <param name="username"></param>
    /// <returns>True if the username exists, false otherwise.</returns>
    Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken);

    /// <summary>
    /// Checks if the given email exists in the system.
    /// </summary>
    /// <param name="email"></param>
    /// <returns>True if the email exists, false otherwise.</returns>
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken);
}