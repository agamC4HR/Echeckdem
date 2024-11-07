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

        public async Task<IActionResult> CombinedDetailed(int ulev, string uno, string organizationName = null, string site = null, string state = null, string city = null, string category = null, DateTime? startDueDate = null, DateTime? endDueDate = null, DateTime? startPeriod = null, DateTime? endPeriod = null)
        {
            var registrations = await _regService.GetDataAsync(ulev, uno, organizationName, site, state, city, startDueDate, endDueDate, startPeriod, endPeriod);
            var contributions = await _contService.GetDataAsync(ulev, uno, organizationName, site, state, city, startDueDate, endDueDate, startPeriod, endPeriod);
            var returns = await _retService.GetDataAsync(ulev, uno, organizationName, site, state, city, startDueDate, endDueDate, startPeriod, endPeriod);

            var detailedViewModel = new CombinedDetailedViewModel
            {
                Registrations = registrations,
                Contributions = contributions,
                Returns = returns,
                OrganizationName = organizationName,
                Site = site,
                State = state,
                City = city,
                Category = category,
                StartDueDate = startDueDate,
                EndDueDate = endDueDate,
                StartPeriod = startPeriod,
                EndPeriod = endPeriod
            };


            return View("~/Views/DetailedView/CombinedDetailedView.cshtml", detailedViewModel);
        }



    }
}