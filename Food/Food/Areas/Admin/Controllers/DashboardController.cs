using Microsoft.AspNetCore.Mvc;

namespace Food.Areas.admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
