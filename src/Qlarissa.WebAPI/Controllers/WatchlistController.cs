using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Qlarissa.Application.Interfaces;
using Qlarissa.WebAPI.Models.Watchlist;

namespace Qlarissa.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class WatchlistController(IQlarissaUserManager userManager, IWatchlistManager watchlistManager) : ControllerBase
{
    private readonly IQlarissaUserManager _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    private readonly IWatchlistManager _watchlistManager = watchlistManager ?? throw new ArgumentNullException(nameof(watchlistManager));

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<WatchlistMinimal>> AddWatchlistAsync(WatchlistMinimal watchlistMinimal)
    {
        var user = await _userManager.GetAsync(User);
        var addedWatchlist = await _watchlistManager.AddWatchlistAsync(watchlistMinimal.ToDomainEntity(), user.Value.Id);
        return Ok(WatchlistMinimal.FromDomainEntity(addedWatchlist));
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> RemoveWatchlistAsync([FromQuery] int watchlistId)
    {
        var user = await _userManager.GetAsync(User);
        await _watchlistManager.RemoveWatchlistAsync(watchlistId, user.Value.Id);
        return Ok();
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult<WatchlistMinimal>> UpdateWatchlistAsync(WatchlistMinimal watchlist)
    {
        var user = await _userManager.GetAsync(User);
        var updatedWatchlist = await _watchlistManager.UpdateWatchlistAsync(watchlist.ToDomainEntity(), user.Value.Id);
        return Ok(WatchlistMinimal.FromDomainEntity(updatedWatchlist));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> WatchSecurityAsync([FromQuery] int watchlistId, [FromQuery] int securityId)
    {
        var user = await _userManager.GetAsync(User);
        await _watchlistManager.WatchSecurityAsync(watchlistId, securityId, user.Value.Id);
        return Ok();
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> UnwatchSecurityAsync([FromQuery] int watchlistId, [FromQuery] int securityId)
    {
        var user = await _userManager.GetAsync(User);
        await _watchlistManager.UnwatchSecurityAsync(watchlistId, securityId, user.Value.Id);
        return Ok();
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<int>> GetWatchlistIdsForSecurity([FromQuery] int securityId)
    {
        var user = await _userManager.GetAsync(User);
        var watchlistIds = await _watchlistManager.GetWatchlistsForSecurityAsync(securityId, user.Value.Id);
        return Ok(watchlistIds);
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Watchlist>>> GetAllWatchlistsAsync(CancellationToken cancellationToken)
    {
        var user = await _userManager.GetAsync(User);
        var domainWatchlists = await _watchlistManager.GetAllWatchlistsAsync(user.Value.Id, cancellationToken);
        return Ok(domainWatchlists.Select(Watchlist.FromDomainEntity));
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<WatchlistMinimal>>> GetWatchlistsMinimalAsync()
    {
        var user = await _userManager.GetAsync(User);
        var domainWatchlists = await _watchlistManager.GetWatchlistsMinimalAsync(user.Value.Id);
        return Ok(domainWatchlists.Select(WatchlistMinimal.FromDomainEntity));
    }
}
