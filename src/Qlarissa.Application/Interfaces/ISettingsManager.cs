using Qlarissa.Domain.Settings;
using Qlarissa.Domain.Settings.Graph;
using FluentResults;

namespace Qlarissa.Application.Interfaces;

public interface ISettingsManager
{
    Task<IEnumerable<GraphSettingBase>> GetGraphSettingsAsync(string userId);

    Task<GraphSettingBase> GetGraphSettingAsync(string userId, GraphDataSource graphSetting);

    Task AddOrUpdateGraphSettingAsync(string userId, GraphSettingBase graphSetting);

    Task<IReadOnlyList<DisplaySetting>> GetDisplaySettingsAsync(string userId, DeviceProfile deviceProfile);

    Task<FluentResults.Result> SyncDisplaySettingsAsync(
        string userId,
        DeviceProfile deviceProfile,
        IReadOnlyCollection<DisplaySettingChange> changes);
}
