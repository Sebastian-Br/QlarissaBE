using Qlarissa.Domain.Entities.Securities;
using Qlarissa.Infrastructure.DB.Entities;

namespace Qlarissa.Infrastructure.Tests.DB;

public class MapperTests
{
    [Fact]
    public void MapCurrency_FromDomainEntity()
    {
        Domain.Entities.Securities.Currency domainEntity = new() { Id = 1, Symbol = "USD", Name = "US Dollar" };
        var dbEntity = Infrastructure.DB.Entities.Currency.FromDomainEntity(domainEntity);
        Assert.Equal(domainEntity.Id, dbEntity.Id);
        Assert.Equal(domainEntity.Symbol, dbEntity.Symbol);
        Assert.Equal(domainEntity.Name, dbEntity.Name);
    }

    [Fact]
    public void MapCurrency_ToDomainEntity()
    {
        Infrastructure.DB.Entities.Currency dbEntity = new() { Id = 1, Symbol = "USD", Name = "US Dollar" };
        var domainEntity = dbEntity.ToDomainEntity();
        Assert.Equal(dbEntity.Id, domainEntity.Id);
        Assert.Equal(dbEntity.Symbol, domainEntity.Symbol);
        Assert.Equal(dbEntity.Name, domainEntity.Name);
    }
}