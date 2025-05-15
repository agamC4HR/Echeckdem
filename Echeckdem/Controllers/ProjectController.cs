using Microsoft.AspNetCore.Mvc;
using Echeckdem.Services;
using Echeckdem.Models;

using Echeckdem.CustomFolder.ProjectBocw;

namespace Echeckdem.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ProjectBocwService _projectbocwService;
        private readonly TrackerService _trackerService;
        private readonly DbEcheckContext _dbEcheckContext;

        public ProjectController(ProjectBocwService projectBocwService, TrackerService trackerService, DbEcheckContext dbEcheckContext)
        {
            _projectbocwService = projectBocwService;
            _trackerService = trackerService;
            _dbEcheckContext = dbEcheckContext;
        }
        public async Task<IActionResult> Index()
        {
            var uno = HttpContext.Session.GetInt32("UNO");
            if (uno == null) return RedirectToAction("Login", "Index");

            var clientSiteMap = await _projectbocwService.GetUserOrgSiteMapAsync(uno.Value);
            var model = new ProjectBocwViewModel
            {
                Clients = clientSiteMap.Keys.ToList(),
                ClientSiteMap = clientSiteMap,
               //TrackerActions = _trackerService.GetNcActionsForUser(uno.Value)
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

        [HttpGet]
        public IActionResult GetNcActions(string client, string site)
        {
            var uno = HttpContext.Session.GetInt32("UNO");
            if (!uno.HasValue || string.IsNullOrEmpty(client) || string.IsNullOrEmpty(site))
                return BadRequest();

            var oid = _dbEcheckContext.Ncmorgs.FirstOrDefault(o => o.Oname == client)?.Oid;
            var lcode = _dbEcheckContext.Ncmlocs.FirstOrDefault(l => l.Lname == site && l.Oid == oid)?.Lcode;

            if (string.IsNullOrEmpty(oid) || string.IsNullOrEmpty(lcode))
                return NotFound();

            var actions = _trackerService.GetNcActionsForUserByOrgAndSite(uno.Value, oid, lcode);
            return Json(actions);
        }





    }
}

