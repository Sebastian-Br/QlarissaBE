using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qlarissa.Application.Interfaces;
using Qlarissa.Domain.Entities.Securities.Base;
using Qlarissa.WebAPI.Models;

namespace Qlarissa.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class SecurityController(ISecurityManager securityManager) : ControllerBase
{
    private readonly ISecurityManager _securityManager = securityManager ?? throw new ArgumentNullException(nameof(securityManager));

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Add([FromQuery] string tickerSymbol, CancellationToken cancellationToken)
    {
        var result = await _securityManager.AddSecurityAsync(tickerSymbol, cancellationToken);

        if(result.IsFailed)
        {
            return BadRequest(string.Join("; ", result.Errors.Select(e => e.Message)));
        }

        return Ok(result.Value);
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<Models.Security.Base.PubliclyTradedSecurityBase>> Get([FromQuery] int id, CancellationToken cancellationToken)
    {
        var security = await _securityManager.GetSecurityAsync(id, cancellationToken);
        if (security == null)
        {
            return NotFound($"Security with ID '{id}' not found.");
        }

        var webApiModel = Models.Security.Base.PubliclyTradedSecurityBase.FromDomainEntity(security);
        return Ok(webApiModel);
    }
}