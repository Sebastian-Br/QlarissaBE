using Qlarissa.Application.Interfaces;
using Qlarissa.Application.Interfaces.MarketData;
using Qlarissa.Application.Interfaces.Repositories;
using Qlarissa.Domain.Entities.Securities;
using Qlarissa.Domain.Entities.Securities.Base;

namespace Qlarissa.Application;

public sealed class SecurityManager(ISecurityDataProvider securityDataProvider) : ISecurityManager
{
    readonly ISecurityDataProvider _securityDataProvider = securityDataProvider ?? throw new ArgumentNullException(nameof(securityDataProvider));

    public Task<IEnumerable<SearchResult>> SearchSecuritiesAsync(string userQuery)
        => _securityDataProvider.SearchSecuritiesAsync(userQuery);

    Task ISecurityManager.AddSecurityAsync(string securityTickerSymbol)
        => _securityDataProvider.AddSecurityAsync(securityTickerSymbol);
}