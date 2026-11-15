using Qlarissa.Application.Interfaces.Repositories;
using Qlarissa.Domain.Settings.Graph;

namespace Qlarissa.Infrastructure.DB.Repositories;

public sealed class SettingsRepository : ISettingsRepository
{
    public Task<GraphSettingBase> AddOrUpdateGraphSettingAsync(GraphSettingBase graphSetting, string userId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<GraphSettingBase>> GetGraphSettingsAsync(string userId)
    {
        throw new NotImplementedException();
    }
}
