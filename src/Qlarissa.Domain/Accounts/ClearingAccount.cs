
namespace Qlarissa.Domain.Accounts;

public class ClearingAccount : Account
{
    /// <summary>
    /// For a T+2 settlement cycle, this value would be 2.
    /// European banks typically use a T+2 settlement cycle.
    /// US banks typically use a T+1 settlement cycle.
    /// This is not used for automated warnings presently, but it could be used that way in the future.
    /// </summary>
    public int SettlementCycleDays { get; set; }
}
