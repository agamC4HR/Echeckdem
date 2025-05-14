using Microsoft.AspNetCore.Mvc;
using Echeckdem.Services;
using Echeckdem.Models;

using Echeckdem.CustomFolder.ProjectBocw;

namespace Echeckdem.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ProjectBocwService _projectbocwService;

        public ProjectController(ProjectBocwService projectBocwService)
        {
            _projectbocwService = projectBocwService;
        }
        public async Task<IActionResult> Index()
        {
            var uno = HttpContext.Session.GetInt32("UNO");
            if (uno == null) return RedirectToAction("Login", "Index");

            var clientSiteMap = await _projectbocwService.GetUserOrgSiteMapAsync(uno.Value);
            var model = new ProjectBocwViewModel
            {
                Clients = clientSiteMap.Keys.ToList(),
                ClientSiteMap = clientSiteMap
            };

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> GetProjectDetails(string client, string site)
        {
            var uno = HttpContext.Session.GetInt32("UNO");
            if (!uno.HasValue || string.IsNullOrEmpty(client) || string.IsNullOrEmpty(site))
            {
                return BadRequest();
            }

            var details = await _projectbocwService.GetProjectDetailsAsync(uno.Value, client, site);
            if (details == null)
            {
                return NotFound();
            }

            return Json(details);
        }


    }
}
