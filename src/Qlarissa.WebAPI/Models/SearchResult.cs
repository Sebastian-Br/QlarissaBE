using Qlarissa.WebAPI.Models.Security.Base;

namespace Qlarissa.WebAPI.Models;

public class SearchResult
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Symbol { get; set; } = string.Empty;

    public SecurityType SecurityType { get; set; }

    public string Exchange { get; set; } = string.Empty;

    public string ExchangeShortName { get; set; } = string.Empty;

    public static SearchResult FromDomainEntity(Domain.Entities.Securities.SearchResult domainSearchResult)
    {
        return new SearchResult
        {
            Id = domainSearchResult.Id,
            Name = domainSearchResult.Name,
            Symbol = domainSearchResult.Symbol,
            SecurityType = (SecurityType)domainSearchResult.SecurityType,
            Exchange = domainSearchResult.Exchange,
            ExchangeShortName = domainSearchResult.ExchangeShortName
        };
    }
}