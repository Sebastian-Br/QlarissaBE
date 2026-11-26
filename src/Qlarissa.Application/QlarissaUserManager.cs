using FluentResults;
using Qlarissa.Application.Interfaces;
using System.Security.Claims;
using Qlarissa.Application.Interfaces.Authorization;
using Qlarissa.Domain;
using Qlarissa.Application.Repositories;
using Qlarissa.Application.Models;
using Qlarissa.Application.Settings;
using System.Security.Cryptography;
using System.Text;

namespace Qlarissa.Application;

public class QlarissaUserManager(
    IUserRepository userRepository,
    IJwtService jwtService,
    IRefreshTokenRepository refreshTokenRepository,
    AuthTokenOptions authTokenOptions) : IQlarissaUserManager
{
    private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    private readonly IJwtService _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository ?? throw new ArgumentNullException(nameof(refreshTokenRepository));
    private readonly AuthTokenOptions _authTokenOptions = authTokenOptions ?? throw new ArgumentNullException(nameof(authTokenOptions));

    private static string CreateRefreshToken() =>
        Convert.ToBase64String(RandomNumberGenerator.GetBytes(64))
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');

    private static string HashRefreshToken(string token) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));

    private async Task<AuthenticationSession> CreateSessionAsync(QlarissaUser user)
    {
        var now = DateTime.UtcNow;
        var refreshToken = CreateRefreshToken();
        await _refreshTokenRepository.AddAsync(
            user.Id,
            HashRefreshToken(refreshToken),
            now,
            now.AddDays(_authTokenOptions.RefreshTokenLifetimeDays));
        return new AuthenticationSession(_jwtService.GenerateToken(user), refreshToken);
    }

    public async Task<Result> RegisterAsync(string username, string email, int displayCurrencyId, string password)
    {
        var user = new QlarissaUser
        {
            UserName = username,
            Email = email,
            DisplayCurrencyId = displayCurrencyId
        };

        var identityResult = await _userRepository.CreateAsync(user, password);
        if (identityResult.Succeeded)
            return Result.Ok();

        var result = Result.Fail("Failed to register user.");
        result.WithErrors(identityResult.Errors.Select(error => error.Description));
        return result;
    }

    public async Task<Result<AuthenticationSession>> LoginAsync(string username, string password)
    {
        var user = await _userRepository.GetAsync(username);
        if (user == null || !await _userRepository.CheckPasswordAsync(user, password))
            return Result.Fail("Invalid username & password combination or user does not exist.");

        return Result.Ok(await CreateSessionAsync(user));
    }

    public async Task<Result<AuthenticationSession>> RefreshAsync(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return Result.Fail("A valid refresh token is required.");

        var now = DateTime.UtcNow;
        var replacementToken = CreateRefreshToken();
        var userId = await _refreshTokenRepository.RotateAsync(
            HashRefreshToken(refreshToken),
            HashRefreshToken(replacementToken),
            now,
            now.AddDays(_authTokenOptions.RefreshTokenLifetimeDays));
        if (userId is null)
            return Result.Fail("The refresh session is invalid or expired.");

        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
            return Result.Fail("The refresh session is invalid or expired.");

        return Result.Ok(new AuthenticationSession(_jwtService.GenerateToken(user), replacementToken));
    }

    public Task RevokeRefreshTokenAsync(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return Task.CompletedTask;

        return _refreshTokenRepository.RevokeAsync(HashRefreshToken(refreshToken), DateTime.UtcNow);
    }

    public async Task<Result<QlarissaUser>> GetAsync(ClaimsPrincipal user)
    {
        var userId = user?.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? user?.FindFirst("sub")?.Value;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Result.Fail("Error retrieving user.");
        }

        var resultUser = await _userRepository.GetByIdAsync(userId);
        if (resultUser == null)
        {
            return Result.Fail("Error retrieving user.");
        }

        return resultUser;
    }

    public Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken)
        => _userRepository.UsernameExistsAsync(username, cancellationToken);

    public Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken)
        => _userRepository.EmailExistsAsync(email, cancellationToken);
}