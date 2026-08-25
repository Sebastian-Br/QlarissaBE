using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Qlarissa.Application.Interfaces;
using Qlarissa.WebAPI.Models;

namespace Qlarissa.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CurrencyController(ICurrencyManager currencyManager) : ControllerBase
{
    private readonly ICurrencyManager _currencyManager = currencyManager ?? throw new ArgumentNullException(nameof(currencyManager));

    [HttpGet]
    public async Task<IActionResult> GetCurrenciesAsync()
    {
        var currencies = (await _currencyManager.GetCurrenciesAsync()).Select(Currency.FromDomainEntity);
        return Ok(currencies);
    }
}