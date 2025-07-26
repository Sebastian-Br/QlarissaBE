using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qlarissa.Application.Interfaces;

public interface IQlarissaUserManager
{
    public Task<Result> RegisterAsync(string username, string email, string password);
}
