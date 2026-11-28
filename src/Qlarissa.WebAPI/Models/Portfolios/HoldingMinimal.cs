namespace Qlarissa.WebAPI.Models.Portfolios;

public class HoldingMinimal
{
    public int Id { get; set; }

    public Domain.Securities.Base.SecurityType SecurityType { get; set; }

    public int SecurityId { get; set; }

    public string SecurityName { get; set; } = string.Empty;

    public double Quantity { get; set; }

    public static HoldingMinimal FromDomainEntity(Domain.Portfolios.HoldingMinimal domainEntity)
    {
        return new HoldingMinimal
        {
            Id = domainEntity.Id,
            SecurityType = domainEntity.SecurityType,
            SecurityId = domainEntity.SecurityId,
            SecurityName = domainEntity.SecurityName,
            Quantity = domainEntity.Amount
        };
    }
}
