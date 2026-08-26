using Qlarissa.Domain.Entities.Securities;

namespace Qlarissa.Application.Interfaces.MarketData;

public interface ISecurityDataProvider
{
    public Task AddSecurityAsync(string securityTickerSymbol);

    public Task<IEnumerable<SearchResult>> SearchSecuritiesAsync(string userQuery);
}