
using Qlarissa.Domain.Portfolios;

namespace Qlarissa.Domain.Accounts;

public class AccountGroup
{
    public int Id { get; set; }

    public string BankName { get; set; } = string.Empty;

    public string Alias { get; set; } = string.Empty;

    public ClearingAccount ClearingAccount { get; set; }

    public IEnumerable<Portfolio> Portfolios { get; set; } = [];

    public IEnumerable<Account> Accounts { get; set; } = [];
}
