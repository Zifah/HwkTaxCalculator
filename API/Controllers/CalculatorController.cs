using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CalculatorController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
