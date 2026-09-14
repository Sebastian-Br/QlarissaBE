using Qlarissa.Domain;

namespace Qlarissa.Application.Interfaces.Authorization;

public interface IJwtService
{
    string GenerateToken(QlarissaUser user);
}