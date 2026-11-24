namespace Qlarissa.Infrastructure.DB.Entities.Settings.Graph;

public class DividendPayoutGraphSettings : GraphSettingsBase<Domain.Settings.Graph.DividendPayoutGraphSettings>
{
    // public bool ShowDividendYield { get; set; } -- move to tags

    public bool IsSynchronizedWithSecurityGraph { get; set; }

    public DividendPayoutGraphSettings() { }

    // ~ FromDomainEntity
    public DividendPayoutGraphSettings(Domain.Settings.Graph.DividendPayoutGraphSettings? domainEntity, string userId)
    {
        if (domainEntity == null)
            return;

        base.FromDomainEntity(domainEntity, userId);
        IsSynchronizedWithSecurityGraph = domainEntity.IsSynchronizedWithSecurityGraph;
    }

    public new Domain.Settings.Graph.DividendPayoutGraphSettings ToDomainEntity()
    {
        var domainEntity = base.ToDomainEntity();
        domainEntity.IsSynchronizedWithSecurityGraph = IsSynchronizedWithSecurityGraph;
        return domainEntity;
    }

    public void UpdateFromDbEntity(DividendPayoutGraphSettings dbEntity)
    {
        IsSynchronizedWithSecurityGraph = dbEntity.IsSynchronizedWithSecurityGraph;
    }
}
