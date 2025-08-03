using Qlarissa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qlarissa.Infrastructure.Authorization;

public interface IJwtService
{
    string GenerateToken(QlarissaUser user);
}