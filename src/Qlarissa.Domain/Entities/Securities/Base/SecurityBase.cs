namespace Qlarissa.Domain.Entities.Securities.Base;

public abstract class SecurityBase
{
    public required string Name { get; set; }

    public required Currency Currency { get; set; }
}