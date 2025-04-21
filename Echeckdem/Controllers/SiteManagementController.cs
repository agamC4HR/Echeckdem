using Echeckdem.CustomFolder;
using Echeckdem.Models;
using Echeckdem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace Echeckdem.Controllers
{
    public class SiteManagementController : Controller
    {

        private readonly ISiteManagementService _siteManagementService;

        public SiteManagementController(ISiteManagementService siteManagementService)

        {
            _siteManagementService = siteManagementService;
        }

        public async Task<IActionResult> Index()
        {
            var activeOrgs = await _siteManagementService.GetActiveOrganizationsAsync();
            return View(activeOrgs);
        }

        public async Task<IActionResult> ViewLocations(string oid)
        {
            var locations = await _siteManagementService.GetLocationsByOidAsync(oid);
            
            ViewBag.OID = oid;
            
            return View(locations);
        }

        [HttpPost]
        public async Task<IActionResult> ReturnSetup(string oid, string lcode)
        {
            var location = await _siteManagementService.GetLocationDetailsAsync(oid, lcode);
            if (location == null)
            {
                return NotFound();
            }

            var viewModel = new ReturnPeriodSelectionViewModel
            {
                Oid = location.Oid,
                Lcode = location.Lcode,
                Lstate = location.Lstate,
                Ltype = location.Ltype,
                Iscloc = location.Iscloc ?? 0
            };

            return View("SelectReturnPeriod", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> FetchApplicableReturns(ReturnPeriodSelectionViewModel input)
        {
            var returns = await _siteManagementService.GetApplicableReturnsAsync(input);
            input.ApplicableReturns = returns;

        
            return View("SelectReturns", input);
        }

        [HttpPost]
        public async Task<IActionResult> SaveSelectedReturns(ReturnPeriodSelectionViewModel input)
        {
            await _siteManagementService.SaveSelectedReturnsAsync(input);
            return RedirectToAction("ViewLocations", new { oid = input.Oid });
        }

        public async Task<IActionResult> SubmittedReturns(string oid, string lcode)
        {
            var groupedReturns = await _siteManagementService.GetSubmittedReturnsByOrg(oid, lcode);
            return PartialView("_SubmittedReturnsPartial", groupedReturns);
        }

        //---------------------------------------------------------------------------//


        [HttpPost]
        public async Task<IActionResult> ContributionSetup(string oid, string lcode)
        {
            var location = await _siteManagementService.GetLocationDetailsAsync(oid, lcode);
            if (location == null)
            {
                return NotFound();
            }

            var viewModel = new ContributionPeriodSelectionViewModel
            {
                Oid = location.Oid,
                Lcode = location.Lcode,
                Lstate = location.Lstate
                //Ltype = location.Ltype,
                //Iscloc = location.Iscloc ?? 0
            };

            return View("SelectContributionPeriod", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> FetchApplicableContributions(ContributionPeriodSelectionViewModel input)
        {
            var contributions = await _siteManagementService.GetApplicableContributionsAsync(input);
            input.ApplicableContributions = contributions;
           

            return View("SelectContributions", input); // Use a new view or partial for selection UI
        }


        [HttpPost]
        public async Task<IActionResult> SaveSelectedContributions(ContributionPeriodSelectionViewModel input)
        {
            await _siteManagementService.SaveSelectedContributionsAsync(input);
            return RedirectToAction("ViewLocations", new { oid = input.Oid });
        }

        public async Task<IActionResult> SubmittedContributions(string oid, string lcode)
        {
            var groupedContributions = await _siteManagementService.GetSubmittedContributionsByOrg(oid, lcode);
            return PartialView("_SubmittedContributionsPartial", groupedContributions);
        }


    }
}
