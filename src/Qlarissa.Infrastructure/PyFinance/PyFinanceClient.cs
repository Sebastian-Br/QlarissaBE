using Qlarissa.Domain.Entities.Securities;
using Qlarissa.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Qlarissa.Infrastructure.PyFinance;

public class PyFinanceClient : IPyFinanceClient
{
    public Task<IEnumerable<SearchResult>> SearchSecuritiesAsync(string userQuery)
    {
        throw new NotImplementedException();
    }
}