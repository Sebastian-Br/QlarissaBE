namespace Qlarissa.Domain.Accounts;

public abstract class Account
{
    public int Id { get; set; }

    public AccountType Type { get; set; }

    public string Name { get; set; } = string.Empty;

    public string IBAN { get; set; } = string.Empty;

    public Currency AccountCurrency { get; set; }

    public InterestOnDeposits? InterestOnDeposits { get; set; }

    public double Balance { get; set; }
}
