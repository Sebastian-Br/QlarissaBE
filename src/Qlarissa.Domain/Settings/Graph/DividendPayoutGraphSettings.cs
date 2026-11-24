namespace Qlarissa.Domain.Settings.Graph;

public sealed class DividendPayoutGraphSettings : GraphSettingBase
{
    public bool IsSynchronizedWithSecurityGraph { get; set; } = true;
}