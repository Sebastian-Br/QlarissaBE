using Qlarissa.Domain.Settings;
using Qlarissa.Domain.Settings.Graph;

namespace Qlarissa.Application.Interfaces;

public interface ISettingsManager
{
    Task<IEnumerable<GraphSettingBase>> GetGraphSettingsAsync(string userId);

    Task<GraphSettingBase> GetGraphSettingAsync(string userId, GraphDataSource graphSetting);

    Task<SettingsTags<PlotType>> GetPlotSettingAsync(string userId, PlotType plotType);

    Task AddOrUpdateGraphSettingAsync(string userId, GraphSettingBase graphSetting);

    Task AddOrUpdatePlotSettingsTags(string userId, SettingsTags<PlotType> tags);

    Task RemovePlotSettingsTags(string userId, SettingsTags<PlotType> tags);

    Task AddOrUpdateGraphSettingsTags(string userId, SettingsTags<GraphSettingBase> tags);

    Task RemoveGraphSettingsTags(string userId, SettingsTags<GraphSettingBase> tags);
}
