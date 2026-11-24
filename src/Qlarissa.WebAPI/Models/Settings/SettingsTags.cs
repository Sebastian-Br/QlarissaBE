namespace Qlarissa.WebAPI.Models.Settings;

public readonly record struct SettingsTags<T> 
(
    int SettingsTypeId,
    IReadOnlyDictionary<string, string> Tags
) where T : Graph.GraphSettingsBase
{
    public static SettingsTags<T> FromDomainEntity<TDomain>(Domain.Settings.SettingsTags<TDomain> domainEntity) where TDomain : Domain.Settings.Graph.GraphSettingBase
    {
        return new(domainEntity.SettingsTypeId, domainEntity.Tags);
    }

    public Domain.Settings.SettingsTags<TDomain> ToDomainEntity<TDomain>() where TDomain : Domain.Settings.Graph.GraphSettingBase
    {
        return new() { SettingsTypeId = SettingsTypeId, Tags = Tags };
    }
}
