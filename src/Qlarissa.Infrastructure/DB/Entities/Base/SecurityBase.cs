using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Qlarissa.Infrastructure.DB.Entities.Base;

public class SecurityBase
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string ShortName { get; set; } = string.Empty;

    public int CurrencyId { get; set; }

    public Currency Currency { get; set; }

    public SecurityType SecurityType { get; set; }

    protected static void FromDomainEntity(Domain.Securities.Base.SecurityBase domainEntity, SecurityBase dbEntity)
    {
        dbEntity.Id = domainEntity.Id;
        dbEntity.Name = domainEntity.Name;
        dbEntity.ShortName = domainEntity.ShortName;
        dbEntity.CurrencyId = domainEntity.Currency.Id; // When adding a security, the currency must already exist in the database.
        dbEntity.SecurityType = (SecurityType)domainEntity.SecurityType;
    }

    protected static void ToDomainEntity(Domain.Securities.Base.SecurityBase domainEntity, SecurityBase dbEntity)
    {
        domainEntity.Id = dbEntity.Id;
        domainEntity.Name = dbEntity.Name;
        domainEntity.Currency = dbEntity.Currency.ToDomainEntity();
        domainEntity.SecurityType = (Domain.Securities.Base.SecurityType)dbEntity.SecurityType;
        domainEntity.ShortName = dbEntity.ShortName;
    }
}

public class SecurityBaseConfiguration : IEntityTypeConfiguration<SecurityBase>
{
    public void Configure(EntityTypeBuilder<SecurityBase> builder)
    {
        builder.Property(s => s.Id).ValueGeneratedOnAdd();

        builder.HasOne(s => s.Currency)
            .WithMany()
            .HasForeignKey(p => p.CurrencyId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}