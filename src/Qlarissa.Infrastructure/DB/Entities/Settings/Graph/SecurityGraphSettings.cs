
namespace Qlarissa.Infrastructure.DB.Entities.Settings.Graph;

public sealed class SecurityGraphSettings : GraphSettingsBase<Domain.Settings.Graph.SecurityGraphSettings>
{
    public SecurityGraphSettings() { }

    // ~ FromDomainEntity
    public SecurityGraphSettings(Domain.Settings.Graph.SecurityGraphSettings? domainEntity = null, string userId = "")
    {
        if (domainEntity == null)
            return;

        base.FromDomainEntity(domainEntity, userId);
    }

    public override Domain.Settings.Graph.SecurityGraphSettings ToDomainEntity()
    {
        var domainEntity = base.ToDomainEntity();
        return domainEntity;
    }

    public void UpdateFromDbEntity(SecurityGraphSettings domainEntity)
    {
    }
}
