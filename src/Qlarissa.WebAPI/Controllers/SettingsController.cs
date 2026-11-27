using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using Qlarissa.WebAPI.Models.Settings.Graph;

namespace Qlarissa.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public sealed class SettingsController(
    Application.Interfaces.IQlarissaUserManager userManager,
    Application.Interfaces.ISettingsManager settingsManager,
    IOptions<Application.Settings.DisplaySettingsOptions> displaySettingsOptions) : ControllerBase
{
    private readonly Application.Interfaces.IQlarissaUserManager _userManager =
        userManager ?? throw new ArgumentNullException(nameof(userManager));
    private readonly Application.Interfaces.ISettingsManager _settingsManager =
        settingsManager ?? throw new ArgumentNullException(nameof(settingsManager));
    private readonly Application.Settings.DisplaySettingsOptions _displaySettingsOptions =
        displaySettingsOptions.Value;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GraphSettingsBase>>> GetGraphSettingsAsync()
    {
        var user = await _userManager.GetAsync(User);
        var graphSettings = await _settingsManager.GetGraphSettingsAsync(user.Value.Id);
        return Ok(graphSettings.Select(GraphSettingsBase.FromDomainEntity));
    }

    [HttpGet]
    public async Task<ActionResult<GraphSettingsBase>> GetGraphSettingAsync(
        [FromQuery] GraphDataSource graphSetting)
    {
        var user = await _userManager.GetAsync(User);
        var graphSettings = await _settingsManager.GetGraphSettingAsync(
            user.Value.Id,
            (Domain.Settings.Graph.GraphDataSource)graphSetting);
        return Ok(GraphSettingsBase.FromDomainEntity(graphSettings));
    }

    [HttpPut]
    public async Task<IActionResult> AddOrUpdateGraphSettingAsync(
        [FromBody] GraphSettingsBase graphSetting)
    {
        var user = await _userManager.GetAsync(User);
        await _settingsManager.AddOrUpdateGraphSettingAsync(user.Value.Id, graphSetting.ToDomainEntity());
        return Ok();
    }

    [HttpGet]
    [EnableRateLimiting("DisplaySettings")]
    public async Task<ActionResult<Models.Settings.DisplaySettingsSnapshot>>
        GetDisplaySettingsAsync([FromQuery] Domain.Settings.DeviceProfile deviceProfile)
    {
        if (!Enum.IsDefined(deviceProfile))
            return BadRequest("Unknown device profile.");

        var user = await _userManager.GetAsync(User);
        var settings = await _settingsManager.GetDisplaySettingsAsync(user.Value.Id, deviceProfile);
        return Ok(new Models.Settings.DisplaySettingsSnapshot(
            _displaySettingsOptions.SyncDebounceMilliseconds,
            _displaySettingsOptions.MaxChangedKeysPerRequest,
            settings.Select(setting =>
                new Models.Settings.DisplaySettingItem(setting.Key, setting.Value)).ToArray()));
    }

    [HttpPut]
    [EnableRateLimiting("DisplaySettings")]
    public async Task<IActionResult> SyncDisplaySettingsAsync(
        [FromBody] Models.Settings.SyncDisplaySettingsRequest request)
    {
        if (request is null || request.Changes is null)
            return BadRequest("A display settings request with Changes is required.");

        if (!Enum.IsDefined(request.DeviceProfile))
            return BadRequest("Unknown device profile.");

        if (request.Changes.Count > _displaySettingsOptions.MaxChangedKeysPerRequest)
            return BadRequest(
                $"A display settings request may change at most {_displaySettingsOptions.MaxChangedKeysPerRequest} keys.");

        if (request.Changes.Any(change => change is null || change.Key is null))
            return BadRequest("Every change must contain a key.");

        var user = await _userManager.GetAsync(User);
        var result = await _settingsManager.SyncDisplaySettingsAsync(
            user.Value.Id,
            request.DeviceProfile,
            request.Changes.Select(change =>
                new Domain.Settings.DisplaySettingChange(change.Key, change.Value)).ToArray());

        if (result.IsSuccess)
            return Ok();

        var error = string.Join("; ", result.Errors.Select(reason => reason.Message));
        if (error.Contains("quota exceeded", StringComparison.OrdinalIgnoreCase))
            return Conflict(error);
        return BadRequest(error);
    }
}
