namespace Qlarissa.WebAPI.Models.GraphSettings;

public class DividendPayoutGraphSettings : GraphSettingsBase<Domain.GraphSettings.DividendPayoutGraphSettings>
{
    public bool ShowDividendYield { get; set; }

    public bool IsSynchronizedWithSecurityGraph { get; set; }

    public override Domain.GraphSettings.DividendPayoutGraphSettings ToDomainEntity()
    {
        var domainEntity = base.ToDomainEntity();
        domainEntity.ShowDividendYield = ShowDividendYield;
        domainEntity.IsSynchronizedWithSecurityGraph = IsSynchronizedWithSecurityGraph;
        return domainEntity;
    }

    public override void FromDomainEntity(Domain.GraphSettings.DividendPayoutGraphSettings domainEntity)
    {
        base.FromDomainEntity(domainEntity);
        ShowDividendYield = domainEntity.ShowDividendYield;
        IsSynchronizedWithSecurityGraph = domainEntity.IsSynchronizedWithSecurityGraph;
    }
}
