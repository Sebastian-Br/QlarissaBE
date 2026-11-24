using Qlarissa.Application.Interfaces;
using Qlarissa.Application.Repositories;
using Qlarissa.Domain.Settings;
using Qlarissa.Domain.Settings.Graph;

namespace Qlarissa.Application;

public sealed class SettingsManager(ISettingsRepository settingsRepository) : ISettingsManager
{
    private readonly ISettingsRepository _settingsRepository = settingsRepository ?? throw new ArgumentNullException(nameof(settingsRepository));

    public Task<IEnumerable<GraphSettingBase>> GetGraphSettingsAsync(string userId)
        => _settingsRepository.GetGraphSettingsAsync(userId);

    public Task<GraphSettingBase> GetGraphSettingAsync(string userId, GraphDataSource graphSetting)
        => _settingsRepository.GetGraphSettingAsync(userId, graphSetting);

    public Task<SettingsTags<PlotType>> GetPlotSettingAsync(string userId, PlotType plotType)
        => _settingsRepository.GetPlotSettingAsync(userId, plotType);

    public Task AddOrUpdateGraphSettingAsync(string userId, GraphSettingBase graphSetting)
        => _settingsRepository.AddOrUpdateGraphSettingAsync(userId, graphSetting);

    public Task AddOrUpdatePlotSettingsTags(string userId, SettingsTags<PlotType> tags)
        => _settingsRepository.AddOrUpdatePlotSettingsTags(userId, tags);

    public Task RemovePlotSettingsTags(string userId, SettingsTags<PlotType> tags)
        => _settingsRepository.RemovePlotSettingsTags(userId, tags);

    public Task AddOrUpdateGraphSettingsTags(string userId, SettingsTags<GraphSettingBase> tags)
        => _settingsRepository.AddOrUpdateGraphSettingsTags(userId, tags);

    public Task RemoveGraphSettingsTags(string userId, SettingsTags<GraphSettingBase> tags)
        => _settingsRepository.RemoveGraphSettingsTags(userId, tags);
}
