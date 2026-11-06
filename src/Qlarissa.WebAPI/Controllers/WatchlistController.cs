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
    public async Task<IActionResult> AddWatchlistAsync(Watchlist watchlist)
    {
        var user = await _userManager.GetAsync(User);
        await _watchlistManager.AddWatchlistAsync(watchlist.ToDomainEntity(), user.Value.Id);
        return Ok();
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
