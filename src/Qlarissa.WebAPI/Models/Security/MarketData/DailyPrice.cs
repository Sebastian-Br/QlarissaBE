namespace Qlarissa.WebAPI.Models.Security.MarketData;

public sealed class DailyPrice
{
    public int Id { get; set; }
    public double Open { get; set; }
    public double Close { get; set; }
    public double High { get; set; }
    public double Low { get; set; }
    public double Average { get; set; }
    public DateOnly Date { get; set; }

    public static DailyPrice FromDomainEntity(Domain.Entities.Securities.MarketData.DailyPrice domainEntity)
    {
        return new DailyPrice
        {
            Id = domainEntity.Id,
            Open = domainEntity.Open,
            Close = domainEntity.Close,
            High = domainEntity.High,
            Low = domainEntity.Low,
            Average = domainEntity.Average,
            Date = domainEntity.Date
        };
    }
}