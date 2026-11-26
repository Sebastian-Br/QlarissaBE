namespace Qlarissa.WebAPI.Models.Settings;

public sealed record DisplaySettingItem(string Key, string Value);

public sealed record DisplaySettingsSnapshot(
    int SyncDebounceMilliseconds,
    int MaxChangedKeysPerRequest,
    IReadOnlyList<DisplaySettingItem> Settings);

public sealed record DisplaySettingChange(string Key, string? Value);

public sealed record SyncDisplaySettingsRequest(
    Qlarissa.Domain.Settings.DeviceProfile DeviceProfile,
    IReadOnlyList<DisplaySettingChange> Changes);
