using Qlarissa.Domain.Entities.Securities.Base;

namespace Qlarissa.Domain.Entities.Securities.Holdings;

public class Holding
{
    public int Id { get; set; }

    public SecurityType SecurityType { get; set; }

    public int SecurityId { get; set; }

    public string SecurityName { get; set; } = string.Empty;

    public IEnumerable<HoldingEvent> HoldingEvents { get; set; } = [];
}