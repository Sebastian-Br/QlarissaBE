using FluentResults;

namespace Qlarissa.Application.Interfaces;

public interface IQlarissaUserManager
{
    Task<Result<string>> LoginAsync(string username, string password);
    Task<Result> RegisterAsync(string username, string email, string password);
}