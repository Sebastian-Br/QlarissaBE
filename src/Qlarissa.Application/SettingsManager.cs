using Qlarissa.Application.Interfaces;
using Qlarissa.Application.Repositories;
using Qlarissa.Application.Settings;
using Qlarissa.Domain.Settings;
using Qlarissa.Domain.Settings.Graph;

namespace Qlarissa.Application;

public sealed class SettingsManager(ISettingsRepository settingsRepository, DisplaySettingsOptions displaySettingsOptions) : ISettingsManager
{
    private readonly ISettingsRepository _settingsRepository = settingsRepository ?? throw new ArgumentNullException(nameof(settingsRepository));
    private readonly DisplaySettingsOptions _displaySettingsOptions = displaySettingsOptions ?? throw new ArgumentNullException(nameof(displaySettingsOptions));

    public Task<IEnumerable<GraphSettingBase>> GetGraphSettingsAsync(string userId)
        => _settingsRepository.GetGraphSettingsAsync(userId);

    public Task<GraphSettingBase> GetGraphSettingAsync(string userId, GraphDataSource graphSetting)
        => _settingsRepository.GetGraphSettingAsync(userId, graphSetting);

    public Task AddOrUpdateGraphSettingAsync(string userId, GraphSettingBase graphSetting)
        => _settingsRepository.AddOrUpdateGraphSettingAsync(userId, graphSetting);

    public Task<IReadOnlyList<DisplaySetting>> GetDisplaySettingsAsync(string userId, DeviceProfile deviceProfile)
        => _settingsRepository.GetDisplaySettingsAsync(userId, deviceProfile);

    public Task<FluentResults.Result> SyncDisplaySettingsAsync(
        string userId,
        DeviceProfile deviceProfile,
        IReadOnlyCollection<DisplaySettingChange> changes)
    {
        if (changes.Select(change => change.Key).Distinct(StringComparer.Ordinal).Count() != changes.Count)
            return Task.FromResult(FluentResults.Result.Fail("A display settings request cannot contain duplicate keys."));

        foreach (var change in changes)
        {
            if (string.IsNullOrWhiteSpace(change.Key)
                || change.Key.Length > _displaySettingsOptions.MaxKeyLength
                || change.Key.Any(char.IsControl))
            {
                return Task.FromResult(FluentResults.Result.Fail(
                    $"Display setting key must be 1 to {_displaySettingsOptions.MaxKeyLength} printable characters."));
            }

            if (change.Value is null)
                continue;

            if (change.Value.Length > _displaySettingsOptions.MaxValueLength)
                return Task.FromResult(FluentResults.Result.Fail(
                    $"Display setting value exceeds {_displaySettingsOptions.MaxValueLength} characters."));
        }

        return _settingsRepository.SyncDisplaySettingsAsync(userId, deviceProfile, changes, _displaySettingsOptions.MaxRowsPerUser);
    }
}
