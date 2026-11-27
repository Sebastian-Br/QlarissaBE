
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Qlarissa.Infrastructure.DB.Entities.Settings;

public class Bounds
{

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public double? Start { get; set; }

    public double? End { get; set; }

    public Domain.Settings.Bounds ToDomainEntity() => new()
    {
        StartDate = StartDate,
        EndDate = EndDate,
        Start = Start,
        End = End
    };

    public static Bounds? FromDomainEntity(Domain.Settings.Bounds? domainEntity) =>
        domainEntity is null ? null : new()
    {
        StartDate = domainEntity.StartDate,
        EndDate = domainEntity.EndDate,
        Start = domainEntity.Start,
        End = domainEntity.End
    };

    public Bounds? UpdateFromDbEntity(Bounds? dbEntity)
    {
        if (dbEntity is null)
            return null;

        StartDate = dbEntity.StartDate;
        EndDate = dbEntity.EndDate;
        Start = dbEntity.Start;
        End = dbEntity.End;

        return this;
    }
}