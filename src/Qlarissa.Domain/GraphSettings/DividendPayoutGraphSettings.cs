namespace Qlarissa.Domain.GraphSettings;

public sealed class DividendPayoutGraphSettings : GraphSettingBase
{
    public DividendPayoutGraphSettings() => PriceCardHoverDistance = 10;

    public bool ShowDividendYield { get; set; } = true;

    public bool IsSynchronizedWithSecurityGraph { get; set; } = true;
}