namespace Qlarissa.Domain.Entities;

public sealed class Currency
{
    public int Id { get; set; }

    public required string Symbol { get; set; }

    public required string Name { get; set; }
}