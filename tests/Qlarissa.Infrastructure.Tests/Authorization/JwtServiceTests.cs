using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Qlarissa.Application.Settings;
using Qlarissa.Domain;
using Qlarissa.Infrastructure.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace Qlarissa.Infrastructure.Tests.Authorization;

public sealed class JwtServiceTests
{
    [Fact]
    public void GenerateToken_UsesImmutableUserIdAsSubjectAndShortLifetime()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "0123456789012345678901234567890123456789012345678901234567890123",
                ["Jwt:Issuer"] = "test-issuer",
                ["Jwt:Audience"] = "test-audience"
            })
            .Build();
        var service = new JwtService(configuration, Options.Create(new AuthTokenOptions
        {
            AccessTokenLifetimeMinutes = 10
        }));
        var before = DateTime.UtcNow;

        var token = new JwtSecurityTokenHandler().ReadJwtToken(service.GenerateToken(new QlarissaUser
        {
            Id = "immutable-user-id",
            UserName = "username",
            Email = "user@example.com"
        }));

        Assert.Equal("immutable-user-id", token.Subject);
        Assert.True(token.ValidTo <= before.AddMinutes(11));
        Assert.True(token.ValidTo >= before.AddMinutes(9));
    }
}
