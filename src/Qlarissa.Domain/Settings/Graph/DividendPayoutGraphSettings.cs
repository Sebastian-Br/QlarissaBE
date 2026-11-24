namespace Qlarissa.Domain.Settings.Graph;

public sealed class DividendPayoutGraphSettings : GraphSettingBase
{
    public bool ShowDividendYield { get; set; } = true;

    public bool IsSynchronizedWithSecurityGraph { get; set; } = true;
}