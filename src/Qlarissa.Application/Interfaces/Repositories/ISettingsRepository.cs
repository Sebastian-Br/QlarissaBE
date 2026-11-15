using Qlarissa.Domain.Settings.Graph;

namespace Qlarissa.Application.Interfaces.Repositories;

public interface ISettingsRepository
{
    Task<IEnumerable<GraphSettingBase>> GetGraphSettingsAsync(string userId);

    Task<GraphSettingBase> AddOrUpdateGraphSettingAsync(GraphSettingBase graphSetting, string userId);
}
