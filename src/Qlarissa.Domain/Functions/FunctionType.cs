namespace Qlarissa.Domain.Functions;

public enum FunctionType
{
    /// <summary>
    /// Functions used to model growth of a security.
    /// They accept a DateOnly and return a double.
    /// </summary>
    Regression,

    /// <summary>
    /// Functions used to generate alert notifications based on a described condition.
    /// They accept any number of parameters and return a boolean.
    /// When true, the alert becomes active.
    /// </summary>
    Alert,

    /// <summary>
    /// Functions used to assign a score to a security, e.g. to rank them.
    /// They accept a security, and return a double.
    /// </summary>
    Scoring,

    /// <summary>
    /// Functions used to produce a subset from a list of securities.
    /// They accept a list of securities and return a filtered list of securities.
    /// </summary>
    Filtering
}