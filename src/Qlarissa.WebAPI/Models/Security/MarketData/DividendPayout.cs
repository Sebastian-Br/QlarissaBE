namespace Qlarissa.WebAPI.Models.Security.MarketData;

public sealed class DividendPayout
{
    public int Id { get; set; }

    public DateOnly PayoutDate { get; set; }

    public double PayoutAmount { get; set; }

    public static DividendPayout FromDomainEntity(Domain.Entities.Securities.MarketData.DividendPayout domainEntity)
    {
        return new DividendPayout
        {
            Id = domainEntity.Id,
            PayoutDate = domainEntity.PayoutDate,
            PayoutAmount = domainEntity.PayoutAmount
        };
    }
}