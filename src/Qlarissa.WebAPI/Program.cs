using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Qlarissa.Application;
using Qlarissa.Application.Interfaces;
using Qlarissa.Application.Interfaces.Authorization;
using Qlarissa.Application.Interfaces.ExternalAPI;
using Qlarissa.Application.Interfaces.Repositories;
using Qlarissa.Domain;
using Qlarissa.Infrastructure.Authorization;
using Qlarissa.Infrastructure.DB;
using Qlarissa.Infrastructure.DB.Repositories;
using Qlarissa.Infrastructure.PyFinance;
using Qlarissa.Infrastructure.PyFinance.Options;
using Qlarissa.WebAPI.LivePrices;
using System.Text;
using ZiggyCreatures.Caching.Fusion;

namespace Qlarissa.WebAPI;

public class Program
{
    private const string LivePriceHubPath = "/hubs/live-prices";

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;

        // Add services to the container.

        builder.Services.AddFusionCache()
            .TryWithAutoSetup()
            .WithDefaultEntryOptions(opt => opt
                .SetMemoryCacheDuration(TimeSpan.FromMinutes(30))
            );

        builder.Services.AddFusionCache("SearchCache")
            .WithMemoryCache(new MemoryCache(new MemoryCacheOptions
            {
                // stores a maximum of 10000 search queries along with their result collections
                SizeLimit = 10000, // for this to work, you must set the Size property when adding cache entries
                CompactionPercentage = 0.20
            }))
            .WithDefaultEntryOptions(opt => opt
                .SetMemoryCacheDuration(TimeSpan.FromMinutes(30))
            );

        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddIdentityCore<QlarissaUser>()
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddSignInManager()
        .AddUserManager<UserManager<QlarissaUser>>()
        .AddDefaultTokenProviders();

        builder.Services.AddSignalR();
        builder.Services.Configure<LivePriceOptions>(builder.Configuration.GetSection("LivePrices"));
        builder.Services.AddSingleton<YahooLivePriceService>();
        builder.Services.AddSingleton<ILivePriceService>(s => s.GetRequiredService<YahooLivePriceService>());
        builder.Services.AddHostedService(s => s.GetRequiredService<YahooLivePriceService>());
        builder.Services.AddSingleton<LivePriceBroadcaster>();

        builder.Services.AddHostedService(
            sp => sp.GetRequiredService<LivePriceBroadcaster>());

        builder.Services.AddScoped<IJwtService, JwtService>();

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IQlarissaUserManager, QlarissaUserManager>();

        builder.Services.AddScoped<ISecurityRepository, SecurityRepository>();
        builder.Services.AddScoped<ISecurityManager, SecurityManager>();

        builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        builder.Services.AddScoped<ICurrencyManager, CurrencyManager>();

        builder.Services.AddScoped<IWatchlistRepository, WatchlistRepository>();
        builder.Services.AddScoped<IWatchlistManager, WatchlistManager>();

        builder.Services.Configure<PyFinanceOptions>(configuration.GetSection("PyFinance"));

        builder.Services.AddHttpClient("PyFinanceSearch", (sp, client) =>
        {
            var options = sp
                .GetRequiredService<IOptions<PyFinanceOptions>>()
                .Value;

            client.BaseAddress = new Uri(options.SearchAPI.BaseUrl);
        });

        builder.Services.AddHttpClient("PyFinanceMarketData", (sp, client) =>
        {
            var options = sp
                .GetRequiredService<IOptions<PyFinanceOptions>>()
                .Value;

            client.BaseAddress = new Uri(options.MarketDataAPI.BaseUrl);
        });

        builder.Services.AddScoped<IMarketDataClient, PyFinanceClient>();

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var jwtKey = configuration["Jwt:Key"];
        if (string.IsNullOrWhiteSpace(jwtKey) || Encoding.UTF8.GetByteCount(jwtKey) < 32)
        {
            throw new InvalidOperationException("Configure Jwt:Key with a secret of at least 32 UTF-8 bytes using a secure configuration provider.");
        }

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Path.StartsWithSegments(LivePriceHubPath))
                        {
                            var accessToken = context.Request.Query["access_token"];
                            if (!string.IsNullOrEmpty(accessToken))
                            {
                                context.Token = accessToken;
                            }
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        var allowedFrontendOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
        if (allowedFrontendOrigins.Length == 0)
        {
            throw new InvalidOperationException("Configure Cors:AllowedOrigins with one or more exact frontend origins.");
        }

        foreach (var origin in allowedFrontendOrigins)
        {
            if (!Uri.TryCreate(origin, UriKind.Absolute, out var originUri)
                || (originUri.Scheme != Uri.UriSchemeHttp && originUri.Scheme != Uri.UriSchemeHttps)
                || originUri.AbsolutePath != "/"
                || !string.IsNullOrEmpty(originUri.UserInfo)
                || !string.IsNullOrEmpty(originUri.Query)
                || !string.IsNullOrEmpty(originUri.Fragment)
                || origin.Contains('*')
                || !string.Equals(origin, origin.Trim(), StringComparison.Ordinal))
            {
                throw new InvalidOperationException($"Cors:AllowedOrigins entry '{origin}' must be an exact HTTP(S) origin without a path, credentials, query, or fragment.");
            }
        }

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend",
                policy => policy
                    .WithOrigins(allowedFrontendOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials());
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger(); // https://localhost:7145/swagger/v1/swagger.json
            app.UseSwaggerUI(); // https://localhost:7145/swagger
        }

        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseHttpsRedirection();
        app.UseCors("AllowFrontend");
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapHub<LivePriceHub>(LivePriceHubPath);
        app.MapControllers();

        app.Run();
    }
}