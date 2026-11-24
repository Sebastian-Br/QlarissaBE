
namespace Qlarissa.Domain.Settings;

public readonly record struct SettingsTags<T>
(
    int SettingsTypeId,
    IReadOnlyDictionary<string, string> Tags
);
