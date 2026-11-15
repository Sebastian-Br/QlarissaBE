namespace Qlarissa.WebAPI.Models.Portfolios;

public sealed class CurrencyHolding
{
    public required Currency Currency { get; set; }
    public double Amount { get; set; }

    public static CurrencyHolding FromDomainEntity(Domain.Portfolios.CurrencyHolding domainEntity)
    {
        return new CurrencyHolding
        {
            Currency = Currency.FromDomainEntity(domainEntity.Currency),
            Amount = domainEntity.Amount
        };
    }
}
