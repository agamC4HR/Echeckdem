using Microsoft.AspNetCore.Mvc;
using Echeckdem.Services;

namespace Echeckdem.Controllers
{
    public class ContributionController : Controller
    {
        private readonly ContributionService _contService;

        public ContributionController(ContributionService contService)
        {
            _contService = contService;
        }
     
        public async Task<IActionResult> Index(string organizationName = null, string LocationName = null, string StateName = null, string CityName = null)
        {
            int ulev = HttpContext.Session.GetInt32("User Level") ?? 0;
            int uno = HttpContext.Session.GetInt32("UNO") ?? 0;

            if (ulev == 0) // || uno == 1)
            {
                // If session values are missing, redirect to login or show error
                TempData["ErrorMessage"] = "Session has expired. Please log in again.";
                return RedirectToAction("Index", "Login");
            }

            var organizationNames = await _contService.GetOrganizationNamesAsync(uno);
            ViewBag.OrganizationNames = organizationNames;

            var locationNames = string.IsNullOrEmpty(organizationName)
               ? await _contService.GetLocationNamesAsync(uno)
               : await _contService.GetFilteredLocationNamesAsync(uno, organizationName);
            ViewBag.LocationNames = locationNames;

            var StateNames = await _contService.GetStateNamesAsync(uno);
            ViewBag.StateNames = StateNames;

            var CityNames = await _contService.GetCityNamesAsync(uno);
            ViewBag.CityNames = CityNames;

           var ContributionData = await _contService.GetDataAsync(ulev, uno, organizationName, LocationName, StateName, CityName);

            return View("~/Views/DetailedView/Contribution.cshtml", ContributionData);

        }

        [HttpGet]
        public async Task<IActionResult> GetLocations(string organizationName)
        {
            int uno = HttpContext.Session.GetInt32("UNO") ?? 0;

            if (string.IsNullOrEmpty(organizationName))
            {
                return Json(await _contService.GetLocationNamesAsync(uno));
            }

            var locations = await _contService.GetFilteredLocationNamesAsync(uno, organizationName);
            return Json(locations);
        }
    }
} 