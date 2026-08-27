using Qlarissa.Domain.Entities.Securities.Base;

namespace Qlarissa.Domain.Entities.Securities;

public class SearchResult
{
    public string Name { get; set; }

    public string Symbol { get; set; }

    public SecurityType SecurityType { get; set; } 

    public string Exchange { get; set; }

    public string ExchangeShortName { get; set; }
}