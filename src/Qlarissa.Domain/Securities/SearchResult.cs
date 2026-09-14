using Qlarissa.Domain.Securities.Base;

namespace Qlarissa.Domain.Securities;

public class SearchResult
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Symbol { get; set; } = string.Empty;

    public SecurityType SecurityType { get; set; }

    public string Exchange { get; set; } = string.Empty;

    public string ExchangeShortName { get; set; } = string.Empty;
}