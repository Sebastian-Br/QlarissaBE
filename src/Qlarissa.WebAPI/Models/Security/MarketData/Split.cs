namespace Qlarissa.WebAPI.Models.Security.MarketData;

public sealed class Split
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    public double SplitRatio { get; set; }

    public static Split FromDomainEntity(Domain.Entities.Securities.MarketData.Split domainEntity)
    {
        return new Split
        {
            Id = domainEntity.Id,
            Date = domainEntity.Date,
            SplitRatio = domainEntity.SplitRatio
        };
    }
}