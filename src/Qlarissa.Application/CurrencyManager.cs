using Qlarissa.Application.Interfaces;
using Qlarissa.Application.Interfaces.Repositories;
using Qlarissa.Domain.Entities;

namespace Qlarissa.Application;

public class CurrencyManager(ICurrencyRepository currencyRepository) : ICurrencyManager
{
    private readonly ICurrencyRepository _currencyRepository = currencyRepository ?? throw new ArgumentNullException(nameof(currencyRepository));

    public Task AddCurrencyAsync(Currency security)
        => _currencyRepository.AddCurrencyAsync(security);

    public Task<IEnumerable<Currency>> GetCurrenciesAsync()
        => _currencyRepository.GetCurrenciesAsync();

    public Task<Currency?> GetCurrencyAsync(string symbol)
        => _currencyRepository.GetCurrencyAsync(symbol);
}