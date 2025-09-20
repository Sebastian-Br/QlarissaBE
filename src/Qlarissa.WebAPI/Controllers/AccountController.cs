using Microsoft.AspNetCore.Mvc;
using Qlarissa.Application.Interfaces;
using Qlarissa.WebAPI.Models;

namespace Qlarissa.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AccountController(IQlarissaUserManager qlarissaUserManager) : ControllerBase
{
    readonly IQlarissaUserManager _qlarissaUserManager = qlarissaUserManager ?? throw new ArgumentNullException(nameof(qlarissaUserManager));

    [HttpPost]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserRequest request)
    {
        var result = await _qlarissaUserManager.RegisterAsync(request.Username, request.Email, request.Password);
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
}