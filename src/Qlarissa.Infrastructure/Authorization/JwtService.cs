using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Qlarissa.Application.Interfaces.Authorization;
using Qlarissa.Application.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;

namespace Qlarissa.Infrastructure.Authorization;

public sealed class JwtService(IConfiguration configuration, IOptions<AuthTokenOptions> options) : IJwtService
{
    private readonly IConfiguration _configuration = configuration;
    private readonly AuthTokenOptions _options = options.Value;

    public string GenerateToken(Domain.QlarissaUser user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_options.AccessTokenLifetimeMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}