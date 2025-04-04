using Echeckdem.Services;
using Microsoft.AspNetCore.Mvc;

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

    }
}
