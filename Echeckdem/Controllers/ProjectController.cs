using Microsoft.AspNetCore.Mvc;
using Echeckdem.Services;
using Echeckdem.Models;
using System.Text.Json;
using Echeckdem.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Driver.Core.Events;
using Echeckdem.ViewModel.ProjectBocw;

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
            
            ProjectBocwViewModel model2=new ProjectBocwViewModel();
            var _UserlocationBOList = JsonSerializer.Deserialize<List<UserLocation>>(HttpContext.Session.GetString("Userbolocation"));
            model2.ClientList = _UserlocationBOList.Select(x=>new SelectListItem { Value=x.Oid,Text=x.Client}).DistinctBy(x=>x.Value).ToList();

           

            return View(model2);
        }
        [HttpPost]
        public async Task<IActionResult> FetchProjectDets() 
        {
            var _UserlocationBOList = JsonSerializer.Deserialize<List<UserLocation>>(HttpContext.Session.GetString("Userbolocation"));
            ProjectBocwViewModel model2 = new ProjectBocwViewModel();
            
            if (!string.IsNullOrEmpty(Request.Form["clientDropdown"]) && !string.IsNullOrWhiteSpace(Request.Form["clientDropdown"]))
            {
                var selectedoid = Request.Form["clientDropdown"].ToString().Trim();
                ViewBag.SelectedOID = selectedoid;
                model2.ClientList = _UserlocationBOList.Select(x => new SelectListItem { Value = x.Oid, Text = x.Client,Selected=(x.Oid==selectedoid) }).DistinctBy(x => x.Value).ToList();
                if (!string.IsNullOrEmpty(Request.Form["siteDropdown"]) && !string.IsNullOrWhiteSpace(Request.Form["siteDropdown"]))
                {
                    var selectedlcode = Request.Form["siteDropdown"].ToString().Trim();
                    model2.projectDetailsDto = await _projectbocwService.GetProjectDetailsAsync(selectedoid, selectedlcode);
                    ViewBag.SelectedLcode= selectedlcode;
                    
                    model2.SiteList = _UserlocationBOList.Where(x => x.Oid == selectedoid).Select(x => new SelectListItem { Value = x.Lcode, Text = x.Site,Selected=(x.Lcode==selectedlcode) }).DistinctBy(x => x.Value).ToList();

                    return View("Index", model2);

                }
                else 
                {

                    model2.SiteList = _UserlocationBOList.Where(x => x.Oid == selectedoid).Select(x => new SelectListItem { Value = x.Lcode, Text = x.Site }).DistinctBy(x => x.Value).ToList();
                    return View("Index", model2);
                }
                    
            }
            else 
            {
                model2.ClientList = _UserlocationBOList.Select(x => new SelectListItem { Value = x.Oid, Text = x.Client }).DistinctBy(x => x.Value).ToList();
                return View("Index", model2);
            }


              
        }
      



       



    }
}

