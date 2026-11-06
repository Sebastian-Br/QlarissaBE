using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qlarissa.Application.Interfaces;

namespace Qlarissa.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DashboardController(ISecurityManager securityManager, IQlarissaUserManager userManager) : ControllerBase
{
    readonly ISecurityManager _securityManager = securityManager ?? throw new ArgumentNullException(nameof(securityManager));
    readonly IQlarissaUserManager _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

    
}