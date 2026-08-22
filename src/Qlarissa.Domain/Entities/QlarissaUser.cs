using Microsoft.AspNetCore.Identity;

namespace Qlarissa.Domain.Entities;

public sealed class QlarissaUser : IdentityUser
{
    public string DisplayName { get; set; } = string.Empty;

    public int DisplayCurrencyId { get; set; }

    public int? SecondaryDisplayCurrencyId { get; set; }
}