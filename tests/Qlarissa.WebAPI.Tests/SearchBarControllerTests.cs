using System;
using System.Collections.Generic;
using System.Text;

namespace Qlarissa.WebAPI.Tests;

public class SearchBarControllerTests
{
    [Fact]
    public void Map_SearchResult_FromDomainEntity()
    {
        Domain.Securities.SearchResult domainEntity = new()
        {
            Id = 1,
            Name = "SecurityName",
            Symbol = "SNAME",
            SecurityType = Domain.Securities.Base.SecurityType.ETF,
            Exchange = "Exchange",
            ExchangeShortName = "EXCHG",

        };

        var apiModel = Models.SearchResult.FromDomainEntity(domainEntity);

        Assert.NotNull(apiModel);
        Assert.Equal(domainEntity.Id, apiModel.Id);
        Assert.Equal(domainEntity.Name, apiModel.Name);
        Assert.Equal(domainEntity.Symbol, apiModel.Symbol);
        Assert.Equal((int)domainEntity.SecurityType, (int)apiModel.SecurityType);
        Assert.Equal(domainEntity.Exchange, apiModel.Exchange);
        Assert.Equal(domainEntity.ExchangeShortName, apiModel.ExchangeShortName);
    }
}