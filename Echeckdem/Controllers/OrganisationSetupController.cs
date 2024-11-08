using Microsoft.AspNetCore.Mvc;
using Echeckdem.Services;

namespace Echeckdem.Controllers
{
    public class OrganisationSetupController : Controller
    {
       private readonly OrganisationSetupService _organisationsetupservice;

        public OrganisationSetupController (OrganisationSetupService organisationSetupService)

        {
            _organisationsetupservice = organisationSetupService;
        }

        [HttpGet]
        public async Task<IActionResult> List(string searchTerm = "")
        {
            var organisationList = await _organisationsetupservice.GetActiveOrganisationsListAsync(searchTerm);
            ViewData["CurrentFilter"] = searchTerm;
            return View("OrganisationList", organisationList);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Invalid organisation ID.");
            }

            var organisationInfo = await _organisationsetupservice.GetOrganisationGeneralInformationAsync(id);
            if (organisationInfo == null)
            {
                return NotFound();
            }

            return View("Details", organisationInfo);
        }
    }
}








