namespace Qlarissa.Domain.Entities.Securities;

public sealed class Currency
{
    public int Id { get; set; }

    public required string Symbol { get; set; }

    public required string Name { get; set; }
}