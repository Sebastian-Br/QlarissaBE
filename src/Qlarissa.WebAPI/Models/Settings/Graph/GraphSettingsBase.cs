using System.Text.Json.Serialization;

namespace Qlarissa.WebAPI.Models.Settings.Graph;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "DataSource")]
[JsonDerivedType(typeof(SecurityGraphSettings), (int)GraphDataSource.Security)]
[JsonDerivedType(typeof(DividendPayoutGraphSettings), (int)GraphDataSource.DividendPayouts)]
public abstract class GraphSettingsBase
{
    public GraphDataSource DataSource { get; set; }

    public PlotType PlotType { get; set; }

    public Bounds? Bounds { get; set; }

    public static GraphSettingsBase FromDomainEntity(Domain.Settings.Graph.GraphSettingBase domainEntity) => domainEntity switch
    {
        Domain.Settings.Graph.SecurityGraphSettings securityGraphSettings => new SecurityGraphSettings(securityGraphSettings),
        Domain.Settings.Graph.DividendPayoutGraphSettings dividendPayoutGraphSettings => new DividendPayoutGraphSettings(dividendPayoutGraphSettings),
        _ => throw new NotSupportedException($"Unsupported graph settings type: {domainEntity.GetType().Name}")
    };

    public abstract Domain.Settings.Graph.GraphSettingBase ToDomainEntity();
}

public class GraphSettingsBase<TGraphSettings> : GraphSettingsBase where TGraphSettings : Domain.Settings.Graph.GraphSettingBase, new()
{
    public override TGraphSettings ToDomainEntity() => new()
    {
        DataSource = (Domain.Settings.Graph.GraphDataSource)DataSource,
        PlotType = (Domain.Settings.Graph.PlotType)PlotType,
        Bounds = Bounds?.ToDomainEntity()
    };

    internal virtual void FromDomainEntity(TGraphSettings domainEntity)
    {
        DataSource = (GraphDataSource)domainEntity.DataSource;
        PlotType = (PlotType)domainEntity.PlotType;
        Bounds = Qlarissa.WebAPI.Models.Settings.Bounds.FromDomainEntity(domainEntity.Bounds);
    }
}
