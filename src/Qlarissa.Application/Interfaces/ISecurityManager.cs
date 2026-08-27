using Qlarissa.Domain.Entities.Securities;
using Qlarissa.Domain.Entities.Securities.Base;

namespace Qlarissa.Application.Interfaces;

public interface ISecurityManager
{
    public Task AddSecurityAsync(string securityTickerSymbol);

    public Task<IEnumerable<SearchResult>> SearchSecuritiesInternallyAsync(string userQuery);
    public Task<IEnumerable<SearchResult>> SearchSecuritiesExternallyAsync(string userQuery);
}