namespace Qlarissa.Application.Models;

public sealed record AuthenticationSession(string AccessToken, string RefreshToken);
