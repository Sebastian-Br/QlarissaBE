using Qlarissa.Domain.Entities;
using Qlarissa.Infrastructure.Persistence.Interfaces;
using FluentResults;
using Qlarissa.Application.Interfaces;
using Qlarissa.Infrastructure.Authorization;

namespace Qlarissa.Application;

public class QlarissaUserManager(IUserRepository userRepository, IJwtService jwtService) : IQlarissaUserManager
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IJwtService _jwtService = jwtService;

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

    public async Task<Result<string>> LoginAsync(string username, string password)
    {
        var user = await _userRepository.GetAsync(username);
        if (user == null || !await _userRepository.CheckPasswordAsync(user, password))
            return Result.Fail("Invalid username & password combination or user does not exist.");

        return Result.Ok(_jwtService.GenerateToken(user));
    }
}