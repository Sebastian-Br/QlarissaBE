
namespace Qlarissa.Infrastructure.DB.Entities.Settings.Graph;

public sealed class SecurityGraphSettings : GraphSettingsBase<Domain.Settings.Graph.SecurityGraphSettings>
{
    public bool AsTotalReturn { get; set; }

    public SecurityGraphSettings() { }

    // ~ FromDomainEntity
    public SecurityGraphSettings(Domain.Settings.Graph.SecurityGraphSettings? domainEntity = null, string userId = "")
    {
        if (domainEntity == null)
            return;

        FromDomainEntity(domainEntity, userId);
        AsTotalReturn = domainEntity.AsTotalReturn;
    }

    public override Domain.Settings.Graph.SecurityGraphSettings ToDomainEntity()
    {
        var domainEntity = base.ToDomainEntity();
        domainEntity.AsTotalReturn = AsTotalReturn;
        return domainEntity;
    }

    public void UpdateFromDbEntity(SecurityGraphSettings dbEntity)
    {
        AsTotalReturn = dbEntity.AsTotalReturn;
    }
}
