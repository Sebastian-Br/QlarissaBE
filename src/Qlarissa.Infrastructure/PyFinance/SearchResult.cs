namespace Qlarissa.Infrastructure.PyFinance;

public class SearchResult
{
    public string Name { get; set; } = string.Empty;

    public string Symbol { get; set; } = string.Empty;

    public string typeDisp { get; set; } = string.Empty;

    public string exchange { get; set; } = string.Empty;

    public string exchDisp { get; set; } = string.Empty;

    public Domain.Securities.SearchResult ToDomainEntity()
    {
        var domainEntity = new Domain.Securities.SearchResult
        {
            Name = Name,
            Symbol = Symbol,
            Exchange = exchange,
            ExchangeShortName = exchDisp
        };

        if (typeDisp == "Equity")
        {
            domainEntity.SecurityType = Qlarissa.Domain.Securities.Base.SecurityType.Stock;
        }
        else if (typeDisp == "ETF")
        {
            domainEntity.SecurityType = Qlarissa.Domain.Securities.Base.SecurityType.ETF;
        }
        else if (typeDisp == "Cryptocurrency")
        {
            domainEntity.SecurityType = Qlarissa.Domain.Securities.Base.SecurityType.Cryptocurrency;
        }
        else if (typeDisp == "Currency")
        {
            domainEntity.SecurityType = Qlarissa.Domain.Securities.Base.SecurityType.CurrencyPair;
        }
        else
        {
            throw new ArgumentException("Invalid security type");
        }

        return domainEntity;
    }
}