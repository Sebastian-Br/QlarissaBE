using Qlarissa.Domain.Entities;
using Qlarissa.Infrastructure.Persistence.Interfaces;
using FluentResults;
using Qlarissa.Application.Interfaces;

namespace Qlarissa.Application;

public class QlarissaUserManager(IUserRepository userRepository) : IQlarissaUserManager
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result> RegisterAsync(string username, string email, string password)
    {
        var user = new QlarissaUser
        {
            UserName = username,
            Email = email
        };

        var identityResult = await _userRepository.CreateAsync(user, password);

        if (identityResult.Succeeded)
            return Result.Ok();

        var result = Result.Fail("Failed to register user.");
        result.WithErrors(identityResult.Errors.Select(error => error.Description));
        return result;
    }
}