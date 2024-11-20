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

        public async Task<IActionResult> CombinedDetailed(string organizationName = null,  string LocationName = null, string StateName = null, string CityName = null)//(int ulev, int uno, string organizationName = null)
        {

            int ulev = HttpContext.Session.GetInt32("User Level") ?? 0;
            int uno = HttpContext.Session.GetInt32("UNO") ?? 0;


            if (ulev == 0)// || uno == 0)
            {
                // If session values are missing, redirect to login or show error
                TempData["ErrorMessage"] = "Session has expired. Please log in again.";
                return RedirectToAction("Index", "Login");
            }

            var registrations = await _regService.GetDataAsync(ulev, uno, organizationName, LocationName, StateName, CityName);
            var contributions = await _contService.GetDataAsync(ulev, uno, organizationName, LocationName, StateName, CityName);
            var returns = await _retService.GetDataAsync(ulev, uno, organizationName, LocationName, StateName, CityName);

            var detailedViewModel = new CombinedDetailedViewModel
            {
                Registrations = registrations,
                Contributions = contributions,
                Returns = returns,
                OrganizationName = organizationName,
                Site = LocationName,
                State = StateName,
                City = CityName
                
                //Category = category,
                //StartDueDate = startDueDate,
                //EndDueDate = endDueDate,
                //StartPeriod = startPeriod,
                //EndPeriod = endPeriod
            };


            return View("~/Views/DetailedView/CombinedDetailedView.cshtml", detailedViewModel);
        }



    }
}