using Microsoft.AspNetCore.Identity;
using Qlarissa.Domain.Entities.Securities;

namespace Qlarissa.Domain.Entities;

public sealed class QlarissaUser : IdentityUser
{
    public string DisplayName { get; set; } = string.Empty;

    public int DisplayCurrencyId { get; set; }
}