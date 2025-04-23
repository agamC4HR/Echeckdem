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

        //----------------START----------------------------------------------RETURNS------------------------------------------------------------------------------//

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


        //----------------END----------------------------------------------RETURNS---------------------------------------------------------------------------------//
        //----------------START----------------------------------------------CONTRIBUTION-------------------------------------------------------------------------//
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
           

            return View("SelectContributions", input); 
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

        //----------------END----------------------------------------------CONTRIBUTION-------------------------------------------------------------------------//
        //----------------START----------------------------------------------REGISTRATION------------------------------------------------------------------------//

        [HttpPost]
        public async Task<IActionResult> RegistrationSetup(string oid, string lcode)
        {
            var location = await _siteManagementService.GetLocationDetailsAsync(oid, lcode);
            if (location == null)
            {
                return NotFound();
            }

            var applicableRegistrations = await _siteManagementService
                .GetApplicableRegistrationsAsync(location.Ltype?.Trim(), location.Lstate);

            var viewModel = new RegistrationSelectionViewModel
            {
                Oid = location.Oid,
                Lcode = location.Lcode,
                Lstate = location.Lstate,
                Ltype = location.Ltype,
                ApplicableRegistrations = applicableRegistrations
            };

            return View("SelectRegistrationPeriod", viewModel);
        }

        // Handles saving of selected registration types
        [HttpPost]
        public async Task<IActionResult> FetchApplicableRegistrations(RegistrationSelectionViewModel model)
        {
            if (model == null || model.ApplicableRegistrations == null || !model.ApplicableRegistrations.Any(r => r.Selected))
            {
                // Log or handle no selections case
                return View("SelectRegistrationPeriod", model); // or redirect with an error message
            }

            await _siteManagementService.SaveSelectedRegistrationsAsync(model);

            // Optional: Use TempData to show a success message after redirect
            TempData["SuccessMessage"] = "Registrations saved successfully.";

            return RedirectToAction("ViewLocations", new { oid = model.Oid });
        }

        [HttpGet]
        public async Task<IActionResult> SubmittedRegistrations(string oid, string lcode)
        {
            var groupedRegistrations = await _siteManagementService.GetSubmittedRegistrationsAsync(oid, lcode);
            return PartialView("_SubmittedRegistrationsPartial", groupedRegistrations);
        }

        //----------------END----------------------------------------------REGISTRATION--------------------------------------------------------------------------//

    }
}
