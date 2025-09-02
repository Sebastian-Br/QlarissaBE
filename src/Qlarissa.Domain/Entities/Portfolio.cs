using Qlarissa.Domain.Entities.Securities;
using Qlarissa.Domain.Entities.Securities.Holdings;

namespace Qlarissa.Domain.Entities;

public class Portfolio
{
    public ICollection<StockHolding>? Stocks { get; set; }
    public ICollection<ETFHolding>? ETFs { get; set; }
    public ICollection<CommodityHolding>? Commodities { get; set; }
    public ICollection<CurrencyHolding>? Currencies { get; set; }

    /// <summary>
    /// The currency of the associated bank account.
    /// </summary>
    public required Currency AccountCurrency { get; set; }
}