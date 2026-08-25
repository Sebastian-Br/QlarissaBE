using Microsoft.AspNetCore.Mvc;
using Qlarissa.Application.Interfaces;
using Qlarissa.WebAPI.Models;

namespace Qlarissa.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AccountController(IQlarissaUserManager userManager) : ControllerBase
{
    readonly IQlarissaUserManager _qlarissaUserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

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
            return Ok(result.Value);

        return Unauthorized(result.Errors.Select(e => e.Message));
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