namespace Qlarissa.Domain.Functions;

public class FunctionSettings
{
    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public FunctionType FunctionType { get; set; }
}