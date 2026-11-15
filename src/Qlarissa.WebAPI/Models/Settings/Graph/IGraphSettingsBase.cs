using System.Text.Json.Serialization;

namespace Qlarissa.WebAPI.Models.Settings.Graph;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "GraphType")]
[JsonDerivedType(typeof(SecurityGraphSettings), (int)GraphType.Security)]
[JsonDerivedType(typeof(DividendPayoutGraphSettings), (int)GraphType.DividendPayouts)]
public interface IGraphSettingsBase
{
    public static IGraphSettingsBase FromDomainEntity(Domain.Settings.Graph.GraphSettingBase domainEntity) => domainEntity switch
    {
        Domain.Settings.Graph.SecurityGraphSettings securityGraphSettings => new SecurityGraphSettings(securityGraphSettings),
        Domain.Settings.Graph.DividendPayoutGraphSettings dividendPayoutGraphSettings => new DividendPayoutGraphSettings(dividendPayoutGraphSettings),
        _ => throw new NotSupportedException($"Unsupported graph settings type: {domainEntity.GetType().Name}")
    };
}