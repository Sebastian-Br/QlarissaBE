using Qlarissa.Domain.Settings;
using Qlarissa.Domain.Settings.Graph;
using FluentResults;

namespace Qlarissa.Application.Repositories;

public interface ISettingsRepository
{
    Task<IEnumerable<GraphSettingBase>> GetGraphSettingsAsync(string userId);

    Task<GraphSettingBase> GetGraphSettingAsync(string userId, GraphDataSource graphSetting);

    Task AddOrUpdateGraphSettingAsync(string userId, GraphSettingBase graphSetting);

    Task<IReadOnlyList<DisplaySetting>> GetDisplaySettingsAsync(string userId, DeviceProfile deviceProfile);

    Task<FluentResults.Result> SyncDisplaySettingsAsync(
        string userId,
        DeviceProfile deviceProfile,
        IReadOnlyCollection<DisplaySettingChange> changes,
        int maxRowsPerUser);
}
