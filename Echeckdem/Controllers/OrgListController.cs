using Echeckdem.Services;
using Microsoft.AspNetCore.Mvc;

namespace Echeckdem.Controllers
{
    public class OrgListController : Controller
    {

        private readonly OrgListService _orglistService;
        public OrgListController(OrgListService orglistService)
        {
            _orglistService = orglistService;
        }



        [HttpGet]
        public async Task<IActionResult> OrganisationList(string searchTerm = "")
        {
            var organisation = await _orglistService.GetActiveOrganisationsListAsync(searchTerm);
            ViewData["CurrentFilter"] = searchTerm;
            return View("OrganisationList", organisation);
        }

        
    }
}
