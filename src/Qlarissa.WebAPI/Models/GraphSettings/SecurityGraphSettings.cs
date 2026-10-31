namespace Qlarissa.WebAPI.Models.GraphSettings;

public sealed class SecurityGraphSettings : GraphSettingsBase<Domain.GraphSettings.SecurityGraphSettings>
{
    public override Domain.GraphSettings.SecurityGraphSettings ToDomainEntity()
    {
        var domainEntity = base.ToDomainEntity();
        return domainEntity;
    }
}