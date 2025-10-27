using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Qlarissa.Infrastructure.DB.Entities;
using QlarissaUser = Qlarissa.Domain.Entities.QlarissaUser;

namespace Qlarissa.Infrastructure.DB;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<QlarissaUser>(options)
{
    public DbSet<Portfolio> Portfolios { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<DailyPrice> DailyPrices { get; set; }
    public DbSet<StockHolding> StockHoldings { get; set; }
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<ETF> ETFs { get; set; }
    public DbSet<ETFHolding> ETFHoldingss { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(decimal) || property.ClrType == typeof(decimal?))
                {
                    property.SetPrecision(18);
                    property.SetScale(6);
                }
            }
        }

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information).EnableSensitiveDataLogging();
    }
}