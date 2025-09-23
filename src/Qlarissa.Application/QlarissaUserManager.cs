using Qlarissa.Domain.Entities;
using FluentResults;
using Qlarissa.Application.Interfaces;
using Qlarissa.Infrastructure.Authorization;
using Qlarissa.Infrastructure.DB.Repositories.Interfaces;
using System.Security.Claims;

namespace Qlarissa.Application;

public class QlarissaUserManager(IUserRepository userRepository, IJwtService jwtService) : IQlarissaUserManager
{
    private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    private readonly IJwtService _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));

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

    public async Task<Result<QlarissaUser>> GetAsync(ClaimsPrincipal user)
    {
        if (user?.Identity?.Name == null)
        {
            return Result.Fail("Error retrieving user.");
        }

        var resultUser = await _userRepository.GetAsync(user.Identity.Name);
        if (resultUser == null)
        {
            return Result.Fail("Error retrieving user.");
        }

        return resultUser;
    }
}