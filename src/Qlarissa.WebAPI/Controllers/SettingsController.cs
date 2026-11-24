using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qlarissa.Application.Interfaces;
using Qlarissa.WebAPI.Models.Settings;
using Qlarissa.WebAPI.Models.Settings.Graph;

namespace Qlarissa.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public sealed class SettingsController(IQlarissaUserManager userManager, ISettingsManager settingsManager) : ControllerBase
{
    private readonly IQlarissaUserManager _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    private readonly ISettingsManager _settingsManager = settingsManager ?? throw new ArgumentNullException(nameof(settingsManager));

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GraphSettingsBase>>> GetGraphSettingsAsync()
    {
        var user = await _userManager.GetAsync(User);
        var graphSettings = await _settingsManager.GetGraphSettingsAsync(user.Value.Id);
        return Ok(graphSettings.Select(GraphSettingsBase.FromDomainEntity));
    }

    [HttpGet]
    public async Task<ActionResult<GraphSettingsBase>> GetGraphSettingAsync([FromQuery] GraphDataSource graphSetting)
    {
        var user = await _userManager.GetAsync(User);
        var graphSettings = await _settingsManager.GetGraphSettingAsync(user.Value.Id, (Domain.Settings.Graph.GraphDataSource)graphSetting);
        return Ok(GraphSettingsBase.FromDomainEntity(graphSettings));
    }

    [HttpGet]
    public async Task<ActionResult<SettingsTags<PlotType>>> GetPlotSettingAsync([FromQuery] PlotType plotType)
    {
        var user = await _userManager.GetAsync(User);
        var tags = await _settingsManager.GetPlotSettingAsync(user.Value.Id, (Domain.Settings.Graph.PlotType)plotType);
        return Ok(SettingsTags<PlotType>.FromDomainEntity(tags));
    }

    [HttpPut]
    public async Task<IActionResult> AddOrUpdateGraphSettingAsync([FromBody] GraphSettingsBase graphSetting)
    {
        var user = await _userManager.GetAsync(User);
        await _settingsManager.AddOrUpdateGraphSettingAsync(user.Value.Id, graphSetting.ToDomainEntity());
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> AddOrUpdatePlotSettingsTags([FromBody] SettingsTags<PlotType> tags)
    {
        var user = await _userManager.GetAsync(User);
        await _settingsManager.AddOrUpdatePlotSettingsTags(user.Value.Id, tags.ToDomainEntity<Domain.Settings.Graph.PlotType>());
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> RemovePlotSettingsTags([FromBody] SettingsTags<PlotType> tags)
    {
        var user = await _userManager.GetAsync(User);
        await _settingsManager.RemovePlotSettingsTags(user.Value.Id, tags.ToDomainEntity<Domain.Settings.Graph.PlotType>());
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> AddOrUpdateGraphSettingsTags([FromBody] SettingsTags<GraphSettingsBase> tags)
    {
        var user = await _userManager.GetAsync(User);
        await _settingsManager.AddOrUpdateGraphSettingsTags(user.Value.Id, tags.ToDomainEntity<Domain.Settings.Graph.GraphSettingBase>());
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveGraphSettingsTags([FromBody] SettingsTags<GraphSettingsBase> tags)
    {
        var user = await _userManager.GetAsync(User);
        await _settingsManager.RemoveGraphSettingsTags(user.Value.Id, tags.ToDomainEntity<Domain.Settings.Graph.GraphSettingBase>());
        return Ok();
    }
}
