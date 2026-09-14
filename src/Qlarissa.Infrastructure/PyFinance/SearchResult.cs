using Qlarissa.Domain.Securities.Base;

namespace Qlarissa.Infrastructure.PyFinance;

public class SearchResult
{
    public string Name { get; set; }

    public string Symbol { get; set; }

    public string typeDisp { get; set; }

    public string exchange { get; set; }

    public string exchDisp { get; set; }

    public Domain.Securities.SearchResult ToDomainEntity()
    {
        var domainEntity = new Domain.Entities.Securities.SearchResult
        {
            Name = Name,
            Symbol = Symbol,
            Exchange = exchange,
            ExchangeShortName = exchDisp
        };

        if (typeDisp == "Equity")
        {
            domainEntity.SecurityType = SecurityType.Stock;
        }
        else if (typeDisp == "ETF")
        {
            domainEntity.SecurityType = SecurityType.ETF;
        }
        else if (typeDisp == "Cryptocurrency")
        {
            domainEntity.SecurityType = SecurityType.Cryptocurrency;
        }
        else if (typeDisp == "Currency")
        {
            domainEntity.SecurityType = SecurityType.CurrencyPair;
        }
        else
        {
            throw new ArgumentException("Invalid security type");
        }

        return domainEntity;
    }
}