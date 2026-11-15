
namespace Qlarissa.Infrastructure.DB.Entities.Settings.Graph;

public interface IGraphSettingsBase
{
    public static IGraphSettingsBase FromDomainEntity(Domain.Settings.Graph.GraphSettingBase domainEntity, string userId) => domainEntity switch
    {
        Domain.Settings.Graph.SecurityGraphSettings securityGraphSettings => new SecurityGraphSettings(securityGraphSettings, userId),
        Domain.Settings.Graph.DividendPayoutGraphSettings dividendPayoutGraphSettings => new DividendPayoutGraphSettings(dividendPayoutGraphSettings, userId),
        _ => throw new NotSupportedException($"Unsupported graph settings type: {domainEntity.GetType().Name}")
    };
}