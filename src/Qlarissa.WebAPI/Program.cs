using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Qlarissa.Application.Interfaces.Authorization;
using Qlarissa.Application.Repositories;
using Qlarissa.Infrastructure.Authorization;
using Qlarissa.Infrastructure.DB;
using Qlarissa.Infrastructure.DB.Repositories;
using Qlarissa.Infrastructure.PyFinance;
using Qlarissa.Infrastructure.PyFinance.Options;
using Qlarissa.WebAPI.LivePrices;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;
using ZiggyCreatures.Caching.Fusion;

namespace Qlarissa.WebAPI;

public class Program
{
    private const string LivePriceHubPath = "/hubs/live-prices";

    private static void AddDisplaySettingsRateLimiter(IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.AddPolicy("DisplaySettings", context =>
            {
                var limits = context.RequestServices
                    .GetRequiredService<IOptions<Application.Settings.DisplaySettingsOptions>>()
                    .Value;
                var userKey = context.User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? context.User.FindFirst("sub")?.Value
                    ?? context.User.FindFirstValue(ClaimTypes.Email)
                    ?? context.User.FindFirst("email")?.Value
                    ?? context.Connection.RemoteIpAddress?.ToString()
                    ?? "unknown";
                return RateLimitPartition.GetFixedWindowLimiter(userKey, _ =>
                    new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = limits.RequestsPerWindow,
                        Window = TimeSpan.FromSeconds(limits.RateLimitWindowSeconds),
                        QueueLimit = 0,
                        AutoReplenishment = true
                    });
            });
        });
    }

    private static void UseDisplaySettingsRequestSizeLimit(WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            if (context.Request.Path.Equals("/api/Settings/SyncDisplaySettings", StringComparison.OrdinalIgnoreCase))
            {
                var maxRequestBytes = context.RequestServices
                    .GetRequiredService<IOptions<Application.Settings.DisplaySettingsOptions>>()
                    .Value.MaxRequestBodyBytes;
                var requestBodySize = context.Features.Get<Microsoft.AspNetCore.Http.Features.IHttpMaxRequestBodySizeFeature>();
                if (requestBodySize is not null && !requestBodySize.IsReadOnly)
                    requestBodySize.MaxRequestBodySize = maxRequestBytes;

                if (context.Request.ContentLength > maxRequestBytes)
                {
                    context.Response.StatusCode = StatusCodes.Status413PayloadTooLarge;
                    return;
                }
            }

            await next();
        });
    }

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
        builder.Services.AddOptions<Application.Settings.DisplaySettingsOptions>()
            .Bind(configuration.GetSection(Application.Settings.DisplaySettingsOptions.SectionName))
            .Validate(options => options.IsValid, "DisplaySettings configuration contains an invalid limit.")
            .ValidateOnStart();
        builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<Application.Settings.DisplaySettingsOptions>>().Value);
        builder.Services.AddOptions<Application.Settings.AuthTokenOptions>()
            .Bind(configuration.GetSection(Application.Settings.AuthTokenOptions.SectionName))
            .Validate(options => options.IsValid, "JWT lifetime settings contain an invalid limit.")
            .ValidateOnStart();
        builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<Application.Settings.AuthTokenOptions>>().Value);
        AddDisplaySettingsRateLimiter(builder.Services);
        builder.Services.AddIdentityCore<Domain.QlarissaUser>()
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddSignInManager()
        .AddUserManager<UserManager<Domain.QlarissaUser>>()
        .AddDefaultTokenProviders();

        builder.Services.AddSignalR();
        builder.Services.Configure<LivePriceOptions>(builder.Configuration.GetSection("LivePrices"));
        builder.Services.AddSingleton<YahooLivePriceService>();
        builder.Services.AddSingleton<Application.Interfaces.ExternalAPI.ILivePriceService>(s => s.GetRequiredService<YahooLivePriceService>());
        builder.Services.AddHostedService(s => s.GetRequiredService<YahooLivePriceService>());
        builder.Services.AddSingleton<LivePriceBroadcaster>();

        builder.Services.AddHostedService(
            sp => sp.GetRequiredService<LivePriceBroadcaster>());

        builder.Services.AddScoped<IJwtService, JwtService>();

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        builder.Services.AddScoped<Application.Interfaces.IQlarissaUserManager, Application.QlarissaUserManager>();

        builder.Services.AddScoped<ISecurityRepository, SecurityRepository>();
        builder.Services.AddScoped<Application.Interfaces.ISecurityManager, Application.SecurityManager>();

        builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        builder.Services.AddScoped<Application.Interfaces.ICurrencyManager, Application.CurrencyManager>();

        builder.Services.AddScoped<IWatchlistRepository, WatchlistRepository>();
        builder.Services.AddScoped<Application.Interfaces.IWatchlistManager, Application.WatchlistManager>();

        builder.Services.AddScoped<ISettingsRepository, SettingsRepository>();
        builder.Services.AddScoped<Application.Interfaces.ISettingsManager, Application.SettingsManager>();

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

        builder.Services.AddScoped<Application.Interfaces.ExternalAPI.IMarketDataClient, PyFinanceClient>();

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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    NameClaimType = ClaimTypes.Name,
                    RoleClaimType = ClaimTypes.Role
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
        UseDisplaySettingsRequestSizeLimit(app);
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("AllowFrontend");
        app.UseAuthentication();
        app.UseRateLimiter();
        app.UseAuthorization();
        app.MapHub<LivePriceHub>(LivePriceHubPath, options =>
            options.CloseOnAuthenticationExpiration = true);
        app.MapControllers();

        app.Run();
    }
}