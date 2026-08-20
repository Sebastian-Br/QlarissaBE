namespace Qlarissa.Domain.Entities.Securities.MarketData;

public sealed class AnalystRating
{
    public int NumberOfAnalystOpinions { get; set; }

    public double TargetMeanPrice { get; set; }

    public double RecommendationMean {  get; set; }
}