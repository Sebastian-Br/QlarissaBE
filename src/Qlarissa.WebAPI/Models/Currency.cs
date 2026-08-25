namespace Qlarissa.WebAPI.Models;

public sealed class Currency
{
    public int Id { get; set; }

    public required string Symbol { get; set; }

    public required string Name { get; set; }

    public static Currency FromDomainEntity(Domain.Entities.Currency currency)
    {
        return new Currency
        {
            Id = currency.Id,
            Symbol = currency.Symbol,
            Name = currency.Name
        };
    }
}