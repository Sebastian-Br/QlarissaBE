namespace Qlarissa.Domain.Portfolios;

public class Portfolio
{
    public int Id { get; set; }

    public IEnumerable<Holding> Holdings { get; set; } = [];
}