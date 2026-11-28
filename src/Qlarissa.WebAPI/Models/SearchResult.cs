namespace Qlarissa.WebAPI.Models;

public class SearchResult
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Symbol { get; set; } = string.Empty;

    public Domain.Securities.Base.SecurityType SecurityType { get; set; }

    public string Exchange { get; set; } = string.Empty;

    public string ExchangeShortName { get; set; } = string.Empty;

    public static SearchResult FromDomainEntity(Domain.Securities.SearchResult domainSearchResult)
    {
        return new SearchResult
        {
            Id = domainSearchResult.Id,
            Name = domainSearchResult.Name,
            Symbol = domainSearchResult.Symbol,
            SecurityType = domainSearchResult.SecurityType,
            Exchange = domainSearchResult.Exchange,
            ExchangeShortName = domainSearchResult.ExchangeShortName
        };
    }
}