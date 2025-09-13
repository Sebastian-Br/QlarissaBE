namespace Qlarissa.Domain.Entities.Securities.Base;

public abstract class SecurityBase
{
    public int Id { get; set; }

    public string Name { get; set; }

    public Currency Currency { get; set; }
}