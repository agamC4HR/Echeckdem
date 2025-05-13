using Microsoft.AspNetCore.Mvc;

namespace Echeckdem.Controllers
{
    public class ProjectController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
