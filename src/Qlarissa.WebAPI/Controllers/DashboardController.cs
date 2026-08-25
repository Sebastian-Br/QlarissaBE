using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qlarissa.Application.Interfaces;
using Qlarissa.Domain.Entities.Securities;
using Qlarissa.Domain.Entities.Securities.MarketData;

namespace Qlarissa.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DashboardController(ISecurityManager securityManager, IQlarissaUserManager userManager) : ControllerBase
{
    readonly ISecurityManager _securityManager = securityManager ?? throw new ArgumentNullException(nameof(securityManager));
    readonly IQlarissaUserManager _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetBasicInformation()
    {
        var userName = User.Identity?.Name ?? "unknown user";
        var user = await _userManager.GetAsync(User);
        return Ok(new
        {
            Message = $"Hello {userName}!",
            SecretData = $"This is protected data, only available with a valid JWT. Your email is {user.Value.Email}"
        });
    }
}