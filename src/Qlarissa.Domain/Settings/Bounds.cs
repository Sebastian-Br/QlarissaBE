namespace Qlarissa.Domain.Settings;

/// <summary>
/// Represents parameter bounds for graphs or functions.
/// Set StartDate/EndDate when dealing with historical data, and Start/End when the parameter is numerical.
/// A bound that is null is considered unbounded in that direction.
/// </summary>
public class Bounds
{
    public int Id { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public double? Start { get; set; }

    public double? End { get; set; }
}
