using Echeckdem.Models;
using Echeckdem.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Policy;

namespace Echeckdem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _loginService;

        public HomeController(ILogger<HomeController> logger, IUserService loginservice)
        {
            _logger = logger;
            _loginService = loginservice;
        }
               
        public  async Task<IActionResult> Index()
        {
            var model = new CombinedDetailedViewModel();
            

            var uno = HttpContext.Session.GetInt32("UNO");

            if (uno.HasValue)
            {
                var locationTypes = await _loginService.GetUserLocationTypesAsync(uno.Value);
                //bool showSpecialView = locationTypes.All(l => l == "S" || l == "F");

                //ViewBag.ShowSpecialView = showSpecialView;

                var typesSet = locationTypes.Select(l => l.ToUpper()).ToHashSet();

                if (typesSet.All(l => l == "S" || l == "F"))
                {
                    ViewBag.ViewType = "OnlySF";                                    // Show view under < !-- if no site is under BOCW --
                }

                else if (typesSet.All(l=> l == "BO"))
                {
                    ViewBag.ViewType = "OnlyBO";                                     // Show view under <!-- if all sites are under BOCW -->
                }

                else if (typesSet.Contains("BO"))
                {
                    ViewBag.ViewType = "Mixed";   // Show view under <!-- if any site is under BOCW -->
                }
                else
                {
                    ViewBag.ViewType = "Default"; // Fallback if needed
                }
            }   

            return View(model);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}


