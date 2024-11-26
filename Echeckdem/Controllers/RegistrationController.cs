using Echeckdem.Services;
using Microsoft.AspNetCore.Mvc;

namespace Echeckdem.Controllers
{
    public class RegistrationController : Controller

    {

        private readonly RegistrationService _regService;

        public RegistrationController(RegistrationService regService)
        {
            _regService = regService;
        }
        public async Task<IActionResult> Index(string organizationName = null, string LocationName = null, string StateName = null, string CityName = null, DateOnly? StartDueDate = null, DateOnly? EndDueDate = null, DateOnly? StartPeriod = null, DateOnly? EndPeriod = null)
        {

            int ulev = HttpContext.Session.GetInt32("User Level") ?? 0;
            int uno = HttpContext.Session.GetInt32("UNO") ?? 0;

            if (ulev == 0) //|| uno == 1)
            {
                // If session values are missing, redirect to login or show error
                TempData["ErrorMessage"] = "Session has expired. Please log in again.";
                return RedirectToAction("Index", "Login");
            }

            var organizationNames = await _regService.GetOrganizationNamesAsync(uno);
            ViewBag.OrganizationNames = organizationNames;

            var locationNames = string.IsNullOrEmpty(organizationName)
               ? await _regService.GetLocationNamesAsync(uno)
               : await _regService.GetFilteredLocationNamesAsync(uno, organizationName);
            ViewBag.LocationNames = locationNames;
            
            var StateNames = await _regService.GetStateNamesAsync(uno);
            ViewBag.StateNames = StateNames;

            var CityNames = await _regService.GetCityNamesAsync(uno);
            ViewBag.CityNames = CityNames;

            var ReturnData = await _regService.GetDataAsync(ulev, uno, organizationName, LocationName, StateName, CityName, StartDueDate, EndDueDate, StartPeriod, EndPeriod);

            return View("~/Views/DetailedView/Registration.cshtml", ReturnData);

        }

        [HttpGet]
        public async Task<IActionResult> GetLocations(string organizationName)
        {
            int uno = HttpContext.Session.GetInt32("UNO") ?? 0;

            if (string.IsNullOrEmpty(organizationName))
            {
                return Json(await _regService.GetLocationNamesAsync(uno));
            }

            var locations = await _regService.GetFilteredLocationNamesAsync(uno, organizationName);
            return Json(locations);
        }

    }
}
