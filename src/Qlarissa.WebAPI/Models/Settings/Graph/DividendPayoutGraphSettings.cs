namespace Qlarissa.WebAPI.Models.Settings.Graph;

public class DividendPayoutGraphSettings : GraphSettingsBase<Domain.Settings.Graph.DividendPayoutGraphSettings>
{
    public bool IsSynchronizedWithSecurityGraph { get; set; }

    public DividendPayoutGraphSettings(Domain.Settings.Graph.DividendPayoutGraphSettings? domainEntity = null)
    {
        if (domainEntity == null)
            return;

        base.FromDomainEntity(domainEntity);
        IsSynchronizedWithSecurityGraph = domainEntity.IsSynchronizedWithSecurityGraph;
    }

    public new Domain.Settings.Graph.DividendPayoutGraphSettings ToDomainEntity()
    {
        var domainEntity = base.ToDomainEntity();
        domainEntity.IsSynchronizedWithSecurityGraph = IsSynchronizedWithSecurityGraph;
        return domainEntity;
    }
}
