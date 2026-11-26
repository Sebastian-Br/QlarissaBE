using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Qlarissa.Application.Settings;
using Qlarissa.Application.Interfaces;
using Qlarissa.WebAPI.Models;

namespace Qlarissa.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AccountController(
    IQlarissaUserManager userManager,
    IOptions<AuthTokenOptions> authTokenOptions,
    IConfiguration configuration) : ControllerBase
{
    private const string RefreshCookieName = "qlarissa.refresh";
    private const string RefreshCookiePath = "/api/Account";
    readonly IQlarissaUserManager _qlarissaUserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    private readonly AuthTokenOptions _authTokenOptions = authTokenOptions?.Value ?? throw new ArgumentNullException(nameof(authTokenOptions));
    private readonly string[] _allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

    [HttpPost]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserRequest request)
    {
        var result = await _qlarissaUserManager.RegisterAsync(request.Username, request.Email, request.DisplayCurrencyId, request.Password);
        if (result.IsSuccess)
            return Ok();

        string errors = string.Join("\n", result.Errors.Select(e => e.Message));
        return BadRequest(errors);
    }

    [HttpPost]
    public async Task<IActionResult> LoginAsync([FromBody] UserLoginRequest request)
    {
        var result = await _qlarissaUserManager.LoginAsync(request.Username, request.Password);
        if (result.IsSuccess)
        {
            if (Request.Cookies.TryGetValue(RefreshCookieName, out var previousRefreshToken))
                await _qlarissaUserManager.RevokeRefreshTokenAsync(previousRefreshToken);
            SetRefreshCookie(result.Value.RefreshToken);
            return Ok(new AccessTokenResponse(result.Value.AccessToken));
        }

        return Unauthorized(result.Errors.Select(e => e.Message));
    }

    [HttpPost]
    public async Task<IActionResult> RefreshAsync()
    {
        if (!HasAllowedOrigin())
            return Forbid();

        if (!Request.Cookies.TryGetValue(RefreshCookieName, out var refreshToken))
            return Unauthorized();

        var result = await _qlarissaUserManager.RefreshAsync(refreshToken);
        if (result.IsFailed)
        {
            DeleteRefreshCookie();
            return Unauthorized();
        }

        SetRefreshCookie(result.Value.RefreshToken);
        return Ok(new AccessTokenResponse(result.Value.AccessToken));
    }

    [HttpPost]
    public async Task<IActionResult> LogoutAsync()
    {
        if (!HasAllowedOrigin())
            return Forbid();

        try
        {
            if (Request.Cookies.TryGetValue(RefreshCookieName, out var refreshToken))
                await _qlarissaUserManager.RevokeRefreshTokenAsync(refreshToken);
        }
        finally
        {
            DeleteRefreshCookie();
        }
        return NoContent();
    }

    private bool HasAllowedOrigin()
    {
        var origin = Request.Headers.Origin.ToString();
        return !string.IsNullOrEmpty(origin)
            && _allowedOrigins.Contains(origin, StringComparer.Ordinal);
    }

    private void SetRefreshCookie(string token)
    {
        Response.Cookies.Append(RefreshCookieName, token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Path = RefreshCookiePath,
            MaxAge = TimeSpan.FromDays(_authTokenOptions.RefreshTokenLifetimeDays),
            IsEssential = true
        });
    }

    private void DeleteRefreshCookie()
    {
        Response.Cookies.Delete(RefreshCookieName, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Path = RefreshCookiePath
        });
    }

    [HttpGet]
    public async Task<ActionResult<AvailabilityResponse>> CheckUsernameAvailabilityAsync(
        [FromQuery] string username,
        CancellationToken cancellationToken)
    {
        var available = !await _qlarissaUserManager.UsernameExistsAsync(username, cancellationToken);

        return Ok(new AvailabilityResponse { Available = available });
    }

    [HttpGet]
    public async Task<ActionResult<AvailabilityResponse>> CheckEmailAvailabilityAsync(
        [FromQuery] string email,
        CancellationToken cancellationToken)
    {
        var available = !await _qlarissaUserManager.EmailExistsAsync(email, cancellationToken);

        return Ok(new AvailabilityResponse { Available = available });
    }
}