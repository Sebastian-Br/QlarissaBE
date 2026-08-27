namespace Qlarissa.Infrastructure.PyFinance;

public class SearchResult
{
    public string Name { get; set; }

    public string Symbol { get; set; }

    public string SecurityType { get; set; }

    public string Exchange { get; set; }

    public string ExchangeShortName { get; set; }

    public Domain.Entities.Securities.SearchResult ToDomainEntity()
    {
        return new Domain.Entities.Securities.SearchResult
        {
            Name = Name,
            Symbol = Symbol,
            SecurityType = Enum.TryParse<Domain.Entities.Securities.Base.SecurityType>(SecurityType, true, out var securityType) ? securityType : throw new ArgumentException("Invalid security type"),
            Exchange = Exchange,
            ExchangeShortName = ExchangeShortName
        };
    }
}