using FluentResults;
using Qlarissa.Domain.Entities;
using System.Security.Claims;

namespace Qlarissa.Application.Interfaces;

public interface IQlarissaUserManager
{
    Task<Result<string>> LoginAsync(string username, string password);

    Task<Result<QlarissaUser>> GetAsync(ClaimsPrincipal user);

    Task<Result> RegisterAsync(string username, string email, string password);
}