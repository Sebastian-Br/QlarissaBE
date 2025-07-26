using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Qlarissa.Domain.Entities;

namespace Qlarissa.Infrastructure.Persistence;

public sealed class ApplicationDbContext : IdentityDbContext<QlarissaUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
    {
    }


}