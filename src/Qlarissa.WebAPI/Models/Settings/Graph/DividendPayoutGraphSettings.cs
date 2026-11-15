namespace Qlarissa.WebAPI.Models.Settings.Graph;

public class DividendPayoutGraphSettings : GraphSettingsBase<Domain.Settings.Graph.DividendPayoutGraphSettings>
{
    public bool ShowDividendYield { get; set; }

    public bool IsSynchronizedWithSecurityGraph { get; set; }

    public DividendPayoutGraphSettings(Domain.Settings.Graph.DividendPayoutGraphSettings? domainEntity = null)
    {
        if (domainEntity == null)
            return;

        base.FromDomainEntity(domainEntity);
        ShowDividendYield = domainEntity.ShowDividendYield;
        IsSynchronizedWithSecurityGraph = domainEntity.IsSynchronizedWithSecurityGraph;
    }

    public new Domain.Settings.Graph.DividendPayoutGraphSettings ToDomainEntity()
    {
        var domainEntity = base.ToDomainEntity();
        domainEntity.ShowDividendYield = ShowDividendYield;
        domainEntity.IsSynchronizedWithSecurityGraph = IsSynchronizedWithSecurityGraph;
        return domainEntity;
    }
}
