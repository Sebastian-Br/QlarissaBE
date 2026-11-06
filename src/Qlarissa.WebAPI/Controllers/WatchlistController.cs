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
    public async Task<ActionResult<Watchlist>> AddWatchlistAsync(Watchlist watchlist)
    {
        var user = await _userManager.GetAsync(User);
        var addedWatchlist = await _watchlistManager.AddWatchlistAsync(watchlist.ToDomainEntity(), user.Value.Id);
        return Ok(Watchlist.FromDomainEntity(addedWatchlist));
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
    public async Task<IActionResult> UpdateWatchlistAsync(Watchlist watchlist)
    {
        var user = await _userManager.GetAsync(User);
        var updatedWatchlist = await _watchlistManager.UpdateWatchlistAsync(watchlist.ToDomainEntity(), user.Value.Id);
        return Ok(Watchlist.FromDomainEntity(updatedWatchlist));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> WatchSecurityAsync([FromQuery] int watchlistId, [FromQuery] int securityId)
    {
        var user = await _userManager.GetAsync(User);
        await _watchlistManager.WatchSecurityAsync(watchlistId, securityId, user.Value.Id);
        return Ok();
    }
}
