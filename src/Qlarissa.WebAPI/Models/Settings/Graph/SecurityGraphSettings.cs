namespace Qlarissa.WebAPI.Models.Settings.Graph;

public sealed class SecurityGraphSettings : GraphSettingsBase<Domain.Settings.Graph.SecurityGraphSettings>
{
    public bool AsTotalReturn { get; set; }

    public SecurityGraphSettings(Domain.Settings.Graph.SecurityGraphSettings? domainEntity = null)
    {
        if (domainEntity == null)
            return;

        base.FromDomainEntity(domainEntity);
        AsTotalReturn = domainEntity.AsTotalReturn;
    }

    public new Domain.Settings.Graph.SecurityGraphSettings ToDomainEntity()
    {
        var domainEntity = base.ToDomainEntity();
        domainEntity.AsTotalReturn = AsTotalReturn;
        return domainEntity;
    }
}