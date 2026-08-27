namespace Qlarissa.WebAPI.Models;

public class SearchResult
{
    public string Name { get; set; }

    public string Symbol { get; set; }

    public SecurityType SecurityType { get; set; }

    public string Exchange { get; set; }

    public string ExchangeShortName { get; set; }

    public static SearchResult FromDomainEntity(Domain.Entities.Securities.SearchResult domainSearchResult)
    {
        return new SearchResult
        {
            Name = domainSearchResult.Name,
            Symbol = domainSearchResult.Symbol,
            SecurityType = (SecurityType)domainSearchResult.SecurityType,
            Exchange = domainSearchResult.Exchange,
            ExchangeShortName = domainSearchResult.ExchangeShortName
        };
    }
}