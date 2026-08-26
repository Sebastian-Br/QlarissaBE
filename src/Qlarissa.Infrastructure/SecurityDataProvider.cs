using Qlarissa.Application.Interfaces.MarketData;
using Qlarissa.Domain.Entities.Securities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Qlarissa.Infrastructure;

public class SecurityDataProvider : ISecurityDataProvider
{
    public Task AddSecurityAsync(string securityTickerSymbol)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<SearchResult>> SearchSecuritiesAsync(string userQuery)
    {
        throw new NotImplementedException();
    }
}