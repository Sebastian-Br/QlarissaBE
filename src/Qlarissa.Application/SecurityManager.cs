using Qlarissa.Application.Interfaces;
using Qlarissa.Application.Interfaces.MarketData;
using Qlarissa.Application.Interfaces.Repositories;
using Qlarissa.Domain.Entities.Securities;
using Qlarissa.Domain.Entities.Securities.Base;

namespace Qlarissa.Application;

public sealed class SecurityManager(ISecurityDataProvider securityDataProvider) : ISecurityManager
{
    readonly ISecurityDataProvider _securityDataProvider = securityDataProvider ?? throw new ArgumentNullException(nameof(securityDataProvider));

    Task ISecurityManager.AddSecurityAsync(string securityTickerSymbol)
        => _securityDataProvider.AddSecurityAsync(securityTickerSymbol);

    Task<IEnumerable<SearchResult>> ISecurityManager.SearchSecuritiesExternallyAsync(string userQuery)
    {
        throw new NotImplementedException();
    }

    Task<IEnumerable<SearchResult>> ISecurityManager.SearchSecuritiesInternallyAsync(string userQuery)
    {
        throw new NotImplementedException();
    }
}