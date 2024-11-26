using Echeckdem.CustomFolder;
using Echeckdem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace Echeckdem.Controllers
{
    public class ReturnsController : Controller
    {
        private readonly ReturnsService _retservice;

        public ReturnsController(ReturnsService retservice)
        {
            _retservice = retservice;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string organizationName = null, string LocationName = null, string StateName = null, string CityName = null)
        {
            int ulev = HttpContext.Session.GetInt32("User Level") ?? 0;
            int uno = HttpContext.Session.GetInt32("UNO") ?? 0;

         
            if (ulev == 0 ) //|| uno == 1)
            {
                // If session values are missing, redirect to login or show error
                TempData["ErrorMessage"] = "Session has expired. Please log in again.";
                return RedirectToAction("Index", "Login");
            }

            var organizationNames = await _retservice.GetOrganizationNamesAsync(uno);
            ViewBag.OrganizationNames = organizationNames;

            var locationNames = string.IsNullOrEmpty(organizationName)
               ? await _retservice.GetLocationNamesAsync(uno)
               : await _retservice.GetFilteredLocationNamesAsync(uno, organizationName);
            ViewBag.LocationNames = locationNames;

            var StateNames = await _retservice.GetStateNamesAsync(uno);
            ViewBag.StateNames = StateNames;

            var CityNames = await _retservice.GetCityNamesAsync(uno);
            ViewBag.CityNames = CityNames;
            var ReturnData = await _retservice.GetDataAsync(ulev, uno, organizationName, LocationName, StateName, CityName);

            return View("~/Views/DetailedView/Returns.cshtml", ReturnData);
                    
        }
        [HttpGet]
        public async Task<IActionResult> GetLocations(string organizationName)
        {
            int uno = HttpContext.Session.GetInt32("UNO") ?? 0;

            if (string.IsNullOrEmpty(organizationName))
            {
                return Json(await _retservice.GetLocationNamesAsync(uno));                                               
            }

            var locations = await _retservice.GetFilteredLocationNamesAsync(uno, organizationName);
            return Json(locations);
        }
       
    }
}
