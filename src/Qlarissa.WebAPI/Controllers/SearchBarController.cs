using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qlarissa.Application.Interfaces;

namespace Qlarissa.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class SearchBarController(ISecurityManager securityManager) : ControllerBase
{
    readonly ISecurityManager _securityManager = securityManager ?? throw new ArgumentNullException(nameof(securityManager));

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Models.SearchResult>>> SearchSecuritiesInternallyAsync([FromQuery] string userQuery, CancellationToken cancellationToken)
    {
        var searchResults = await _securityManager.SearchSecuritiesInternallyAsync(userQuery, cancellationToken);
        return Ok(searchResults.Select(Models.SearchResult.FromDomainEntity));
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Models.SearchResult>>> SearchSecuritiesExternallyAsync([FromQuery] string userQuery, CancellationToken cancellationToken)
    {
        var searchResults = await _securityManager.SearchSecuritiesExternallyAsync(userQuery, cancellationToken);
        return Ok(searchResults.Select(Models.SearchResult.FromDomainEntity));
    }
}