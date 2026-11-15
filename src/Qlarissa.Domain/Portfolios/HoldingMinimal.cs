using Qlarissa.Domain.Securities.Base;

namespace Qlarissa.Domain.Portfolios;

public class HoldingMinimal
{
    public int Id { get; set; }

    public SecurityType SecurityType { get; set; }

    public int SecurityId { get; set; }

    public string SecurityName { get; set; } = string.Empty;

    public double Amount { get; set; }
}
