using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qlarissa.Application.Interfaces;

namespace Qlarissa.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DashboardController(ISecurityManager securityManager, IQlarissaUserManager userManager) : ControllerBase
{
    readonly ISecurityManager _securityManager = securityManager ?? throw new ArgumentNullException(nameof(securityManager));
    readonly IQlarissaUserManager _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<Models.Watchlist.WatchedSecurityMinimal>> IsSecurityWatchedAsync([FromQuery] int securityId)
    {
        var user = await _userManager.GetAsync(User);
        var watchedSecurity = Models.Watchlist.WatchedSecurityMinimal.FromDomainEntity(await _securityManager.IsSecurityWatchedAsync(securityId, user.Value.Id));
        return Ok(watchedSecurity);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<FluentResults.Result>> WatchSecurityAsync([FromQuery] int securityId, [FromQuery] bool isPrimaryWatchlist)
    {
        var user = await _userManager.GetAsync(User);
        var result = await _securityManager.WatchSecurityAsync(securityId, user.Value.Id, isPrimaryWatchlist);

        if (result.IsSuccess)
        {
            return Ok();
        }

        return BadRequest(string.Join(".", result.Errors.Select(e => e.Message)));
    }

    [HttpDelete]
    [Authorize]
    public async Task<ActionResult<FluentResults.Result>> UnwatchSecurityAsync([FromQuery] int securityId)
    {
        var user = await _userManager.GetAsync(User);
        var result = await _securityManager.UnwatchSecurityAsync(securityId, user.Value.Id);

        if (result.IsSuccess)
        {
            return Ok();
        }

        return BadRequest(string.Join(".", result.Errors.Select(e => e.Message)));
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<Models.Watchlist.UserWatchlists>> GetUserWatchlistsAsync(CancellationToken cancellationToken)
    {
        var user = await _userManager.GetAsync(User);
        var domainWatchlists = await _securityManager.GetWatchlistsAsync(user.Value.Id, cancellationToken);
        var userWatchlists = Models.Watchlist.UserWatchlists.FromDomainEntity(domainWatchlists);
        return Ok(userWatchlists);
    }
}