
namespace Qlarissa.Domain.Entities.Securities.Holdings;

public sealed class ETFHolding
{
    public required ETF ETF { get; set; }

    public int Amount { get; set; }
}