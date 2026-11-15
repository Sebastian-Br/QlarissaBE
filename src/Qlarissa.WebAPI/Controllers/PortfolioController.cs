using Microsoft.AspNetCore.Mvc;

namespace Qlarissa.WebAPI.Controllers;

public class PortfolioController : Controller
{
    public async Task<IActionResult> Index()
    {
        return View();
    }
}
