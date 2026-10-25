using Qlarissa.Domain;

namespace Qlarissa.Application.Interfaces.Repositories;

public interface ICurrencyRepository
{
    Task<Currency?> GetCurrencyAsync(string symbol);

    Task<IEnumerable<Currency>> GetCurrenciesAsync();
}