using Qlarissa.Domain.Entities.Securities.Base;

namespace Qlarissa.Domain.Entities.Securities;

/// <summary>
/// In a CurrencyPair, e.g. USD-EUR, USD would be the primary currency.
/// this.Symbol would be USD whereas this.Currency would refer to the Euro
/// </summary>
public class CurrencyPair : PubliclyTradedSecurityBase
{
}