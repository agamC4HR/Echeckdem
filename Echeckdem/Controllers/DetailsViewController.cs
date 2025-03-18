using Echeckdem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using Echeckdem.Services;
using System.Runtime.CompilerServices;
using Echeckdem.CustomFolder;

namespace Echeckdem.Controllers
{
    public class DetailsViewController : Controller
    {
        private readonly RegistrationService _regService;
        private readonly ContributionService _contService;
        private readonly ReturnsService _retService;


        public DetailsViewController(RegistrationService regService, ContributionService contService, ReturnsService retService)
        {
            _regService = regService;
            _contService = contService;
            _retService = retService;
            
        }

        public async Task<IActionResult> CombinedDetailed(string organizationName = null,  string LocationName = null, string StateName = null, string CityName = null, DateOnly? StartDueDate = null, DateOnly? EndDueDate = null, DateOnly? StartPeriod = null, DateOnly? EndPeriod = null)//(int ulev, int uno, string organizationName = null)
        {

            int ulev = HttpContext.Session.GetInt32("User Level") ?? 0;
            int uno = HttpContext.Session.GetInt32("UNO") ?? 0;


            if (ulev == 0)// || uno == 0)
            {
                // If session values are missing, redirect to login or show error 
                TempData["ErrorMessage"] = "Session has expired. Please log in again.";
                return RedirectToAction("Index", "Login");
            }



            var registrations = await _regService.GetDataAsync(ulev, uno, organizationName, LocationName, StateName, CityName, StartDueDate, EndDueDate, StartPeriod, EndPeriod);
            var contributions = await _contService.GetDataAsync(ulev, uno, organizationName, LocationName, StateName, CityName, StartDueDate, EndDueDate, StartPeriod, EndPeriod);
            var returns = await _retService.GetDataAsync(ulev, uno, organizationName, LocationName, StateName, CityName, StartDueDate, EndDueDate, StartPeriod, EndPeriod);

            var detailedViewModel = new CombinedDetailedViewModel
            {
                Registrations = registrations,
                Contributions = contributions,
                Returns = returns,
                OrganizationName = organizationName,
                SiteName = LocationName,
                StateName = StateName,
                CityName = CityName,
                StartDueDate = StartDueDate,
                EndDueDate = EndDueDate,
                StartPeriod = StartPeriod,
                EndPeriod = EndPeriod
            };

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

            //var ReturnData = await _regService.GetDataAsync(ulev, uno, organizationName, LocationName, StateName, CityName);


            return View("~/Views/DetailedView/CombinedDetailedView.cshtml", detailedViewModel);     
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