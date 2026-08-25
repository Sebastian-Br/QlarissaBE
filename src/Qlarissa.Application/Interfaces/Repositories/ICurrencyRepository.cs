using Qlarissa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Qlarissa.Application.Interfaces.Repositories;

public interface ICurrencyRepository
{
    Task<Currency?> GetCurrencyAsync(string symbol);

    Task AddCurrencyAsync(Currency security);

    Task<IEnumerable<Currency>> GetCurrenciesAsync();
}