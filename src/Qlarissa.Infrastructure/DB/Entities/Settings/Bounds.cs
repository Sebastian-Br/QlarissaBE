
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Qlarissa.Infrastructure.DB.Entities.Settings.Graph;

namespace Qlarissa.Infrastructure.DB.Entities.Settings;

public class Bounds
{
    public int Id { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public double? Start { get; set; }

    public double? End { get; set; }

    public Domain.Settings.Bounds? ToDomainEntity() => new()
    {
        Id = Id,
        StartDate = StartDate,
        EndDate = EndDate,
        Start = Start,
        End = End
    };

    public static Bounds? FromDomainEntity(Domain.Settings.Bounds? domainEntity) =>
        domainEntity is null ? null : new()
    {
        Id = domainEntity.Id,
        StartDate = domainEntity.StartDate,
        EndDate = domainEntity.EndDate,
        Start = domainEntity.Start,
        End = domainEntity.End
    };
}

public class BoundsConfiguration : IEntityTypeConfiguration<Bounds>
{
    public void Configure(EntityTypeBuilder<Bounds> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
    }
}