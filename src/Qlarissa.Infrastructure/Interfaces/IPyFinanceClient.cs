using Qlarissa.Domain.Entities.Securities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Qlarissa.Infrastructure.Interfaces;

public interface IPyFinanceClient
{
    public Task<IEnumerable<SearchResult>> SearchSecuritiesAsync(string userQuery);
}