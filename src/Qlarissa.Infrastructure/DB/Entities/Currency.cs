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
        builder.HasData(
            new Currency { Id = 1, Symbol = "USD", Name = "United States Dollar" },
            new Currency { Id = 2, Symbol = "EUR", Name = "Euro" },
            new Currency { Id = 3, Symbol = "JPY", Name = "Japanese Yen" },
            new Currency { Id = 4, Symbol = "GBP", Name = "British Pound Sterling" },
            new Currency { Id = 5, Symbol = "AUD", Name = "Australian Dollar" },
            new Currency { Id = 6, Symbol = "CAD", Name = "Canadian Dollar" },
            new Currency { Id = 7, Symbol = "CHF", Name = "Swiss Franc" },
            new Currency { Id = 8, Symbol = "CNY", Name = "Chinese Yuan" }
        );
    }
}