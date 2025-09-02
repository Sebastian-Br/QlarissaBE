namespace Qlarissa.Domain.Entities.Securities.MarketData;

public sealed class AnalystRating
{
    public RatingKey RatingKey { get; set; }

    public int NumberOfAnalystOpinions { get; set; }

    public double TargetMeanPrice { get; set; }

    public double RecommendationMean {  get; set; }
}

public enum RatingKey
{
    Buy = 1,
    Hold = 2,
    Sell = 3
}