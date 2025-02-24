using Echeckdem.Models;
using Microsoft.AspNetCore.Mvc;
using Echeckdem.Services;
using Microsoft.AspNetCore.Mvc.Rendering;



namespace Echeckdem.Controllers
{
    public class TrackScopeSetupController : Controller
    {

        private readonly ITrackScopeSetupService _trackscopesetupservice;
        private readonly DbEcheckContext _EcheckContext;

        public TrackScopeSetupController(ITrackScopeSetupService trackscopesetupservice, DbEcheckContext EcheckContext)

        {
            _trackscopesetupservice = trackscopesetupservice;
            _EcheckContext = EcheckContext;
        }

        public ActionResult Create()
        {
            ViewBag.ScopesList = new SelectList(_EcheckContext.BocwScopes.ToList(), "ScopeId", "ScopeName");
            ViewBag.StateList = new SelectList(_EcheckContext.Maststates.ToList(), "Stateid", "Statedesc");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TrackScope trackScope)
        {
            if (ModelState.IsValid)
            {
                _EcheckContext.TrackScopes.Add(trackScope);
                _EcheckContext.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ScopesList = new SelectList(_EcheckContext.BocwScopes.ToList(), "ScopeId", "ScopeName", trackScope.ScopeId);
            ViewBag.StateList = new SelectList(_EcheckContext.Maststates.ToList(), "Stateid", "Statedesc", trackScope.Stateid);

            return View(trackScope);
        }

        //[HttpGet]
        //public IActionResult Create()
        //{
        //    try
        //    {
        //        var scopes = _EcheckContext.BocwScopes.ToList();
        //        var states = _EcheckContext.Maststates.ToList();

        //        if (scopes == null || !scopes.Any())
        //        {
        //            scopes = new List<BocwScope> { new BocwScope { ScopeId = "", ScopeName = "No Scope Available" } };
        //        }

        //        if (states == null || !states.Any())
        //        {
        //            states = new List<Maststate> { new Maststate { Stateid = "", Statedesc = "No State Available" } };
        //        }

        //        ViewBag.Scopes = new SelectList(scopes, "ScopeId", "ScopeName");
        //        ViewBag.States = new SelectList(states, "Stateid", "StateDesc");

        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error fetching data: {ex.Message}");
        //        return View("Error");
        //    }
        //}



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(TrackScope model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _trackscopesetupservice.AddTrackScope(model);
        //        return RedirectToAction("Index");
        //    }

        //    var scopes = _EcheckContext.BocwScopes.ToList();
        //    var states = _EcheckContext.Maststates.ToList();
        //    // Repopulate dropdowns if validation fails
        //    ViewBag.Scopes = new SelectList(scopes, "ScopeId", "ScopeName", model.ScopeId);
        //    ViewBag.States = new SelectList(states, "Stateid", "StateDesc", model.Stateid);
        //    return View(model);
        //}
    }
}
