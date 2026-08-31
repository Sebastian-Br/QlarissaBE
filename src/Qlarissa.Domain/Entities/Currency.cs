namespace Qlarissa.Domain.Entities;

public sealed class Currency
{
    public int Id { get; set; }

    public string Symbol { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
}