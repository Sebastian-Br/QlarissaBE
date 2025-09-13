using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Qlarissa.Infrastructure.DB.Entities;

public sealed class Currency
{
    public int Id { get; set; }

    public string Symbol { get; set; }

    public string Name { get; set; }
    public static Currency FromDomainEntity(Domain.Entities.Securities.Currency domainEntity)
        => new() { Id = domainEntity.Id, Symbol = domainEntity.Symbol, Name = domainEntity.Name };

    public Domain.Entities.Securities.Currency ToDomainEntity() 
        => new() { Id = this.Id, Symbol = this.Symbol, Name = this.Name };
}

public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.Property(s => s.Id).ValueGeneratedOnAdd();
    }
}