using Qlarissa.Domain.Entities;

namespace Qlarissa.Application.Interfaces.Authorization;

public interface IJwtService
{
    string GenerateToken(QlarissaUser user);
}