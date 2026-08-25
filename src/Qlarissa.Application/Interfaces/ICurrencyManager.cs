using Qlarissa.Domain.Entities;

namespace Qlarissa.Application.Interfaces;

public interface ICurrencyManager
{
    Task<Currency?> GetCurrencyAsync(string symbol);

    /// <summary>
    /// Retrieves a list of all available currencies.
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<Currency>> GetCurrenciesAsync();

    Task AddCurrencyAsync(Currency security);
}