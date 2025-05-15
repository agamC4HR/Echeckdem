using Echeckdem.CustomFolder.Dashboard.Registration;
using Echeckdem.CustomFolder.Dashboard;
using Echeckdem.Models;
using Echeckdem.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Policy;
using Echeckdem.ViewModel;
using Echeckdem.Handlers;

namespace Echeckdem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _loginService;
        private readonly ContributionService _contributionService;
        private readonly RegistrationService _registrationService;
        private readonly IDashboardSummary _dashboardSummary;
        public HomeController(ILogger<HomeController> logger, IUserService loginservice, ContributionService contributionService, RegistrationService registrationService,IDashboardSummary dashboardSummary)
        {
            _logger = logger;
            _loginService = loginservice;
            _contributionService = contributionService;
            _registrationService = registrationService;    //var model = new CombinedDetailedViewModel();
            _dashboardSummary = dashboardSummary;
        }

        public async Task<IActionResult> Index(int? selectedYear = null)
        {
            // Retrieve user UNO from session
            var uno = HttpContext.Session.GetInt32("UNO");

            // If UNO is not found, redirect to login
            if (!uno.HasValue)
            {
                return RedirectToAction("Login", "Index");
            }

            // Get the location types for the user
            var locationTypes = await _loginService.GetUserLocationTypesAsync(uno.Value);
            var typesSet = locationTypes.Select(l => l.ToUpper()).ToHashSet();

            // Determine the view type based on location types
            string viewType = "Default"; // Default fallback
            List<ProjectDashboardStatus> projectDashboardStatus = new List<ProjectDashboardStatus>();
            if (typesSet.All(l => l == "S" || l == "F"))
            {
                viewType = "OnlySF"; // Show view when no site is under BOCW
            }
            else if (typesSet.All(l => l == "BO"))
            {
                viewType = "OnlyBO"; // Show view when all sites are under BOCW
               
                projectDashboardStatus = _dashboardSummary.GetDashboardSummary(); 

            }
            else if (typesSet.Contains("BO"))
            {
                viewType = "Mixed"; // Show view when some sites are under BOCW
            }

            // Determine the year to fetch (use selected year or current year)
            int yearToFetch = selectedYear ?? DateTime.Now.Year;

            // Fetch the compliant registrations from the service
            List<CompliantRegistrationViewModel> registrations = new List<CompliantRegistrationViewModel>();
            try
            {
                registrations = await _registrationService.GetCompliantRegistrationsAsync(uno.Value, yearToFetch);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching compliant registrations.");
            }

            // Prepare the view model
            var model = new DashboardViewModel
            {
                ViewType = viewType,
                Registrations = registrations,
                ProjectDashboardStatus=projectDashboardStatus
                
            };
            ViewBag.ViewType = viewType;
            // Pass the model to the view
            return View(model);
        }



























        //public async Task<IActionResult> Index(int? selectedYear = null)
        //{
        //    var uno = HttpContext.Session.GetInt32("UNO");




        //    if (uno.HasValue)
        //    {
        //        var locationTypes = await _loginService.GetUserLocationTypesAsync(uno.Value);
        //        var typesSet = locationTypes.Select(l => l.ToUpper()).ToHashSet();

        //        if (typesSet.All(l => l == "S" || l == "F"))
        //        {
        //            ViewBag.ViewType = "OnlySF";                                    // Show view under < !-- if no site is under BOCW --
        //        }

        //        else if (typesSet.All(l=> l == "BO"))
        //        {
        //            ViewBag.ViewType = "OnlyBO";                                     // Show view under <!-- if all sites are under BOCW -->
        //        }

        //        else if (typesSet.Contains("BO"))
        //        {
        //            ViewBag.ViewType = "Mixed";                                     // Show view under <!-- if any site is under BOCW -->
        //        }
        //        else
        //        {
        //            ViewBag.ViewType = "Default"; // Fallback if needed
        //        }

        //        int yearToFetch = selectedYear ?? DateTime.Now.Year;
        //        var registrations = await _registrationService.GetCompliantRegistrationsAsync(uno.Value, yearToFetch);
        //        ViewBag.Registrations = registrations;


        //    }

        //    return View(); 

        //}




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


