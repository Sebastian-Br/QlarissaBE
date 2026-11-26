namespace Qlarissa.Application.Settings;

public sealed class AuthTokenOptions
{
    public const string SectionName = "Jwt";

    public int AccessTokenLifetimeMinutes { get; set; } = 10;
    public int RefreshTokenLifetimeDays { get; set; } = 30;

    public bool IsValid =>
        AccessTokenLifetimeMinutes is >= 1 and <= 60
        && RefreshTokenLifetimeDays is >= 1 and <= 90;
}
