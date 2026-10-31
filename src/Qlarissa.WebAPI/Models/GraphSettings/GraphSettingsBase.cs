namespace Qlarissa.WebAPI.Models.GraphSettings;

public abstract class GraphSettingsBase<TGraphSettings> where TGraphSettings : Domain.GraphSettings.GraphSettingBase, new()
{
    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public bool LogarithmicScale { get; set; } = true;

    public int PriceCardHoverDistance { get; set; }

    public virtual TGraphSettings ToDomainEntity()
    {
        return new TGraphSettings
        {
            StartDate = StartDate,
            EndDate = EndDate,
            LogarithmicScale = LogarithmicScale,
            PriceCardHoverDistance = PriceCardHoverDistance
        };
    }
}