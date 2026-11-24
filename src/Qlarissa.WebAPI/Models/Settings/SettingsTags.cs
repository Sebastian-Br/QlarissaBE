namespace Qlarissa.WebAPI.Models.Settings;

public readonly record struct SettingsTags<T> 
(
    int SettingsTypeId,
    IReadOnlyDictionary<string, string> Tags
)
{
    public static SettingsTags<T> FromDomainEntity<TDomain>(Domain.Settings.SettingsTags<TDomain> domainEntity)
    {
        return new(domainEntity.SettingsTypeId, domainEntity.Tags);
    }

    public Domain.Settings.SettingsTags<TDomain> ToDomainEntity<TDomain>()
    {
        return new() { SettingsTypeId = SettingsTypeId, Tags = Tags };
    }
}
