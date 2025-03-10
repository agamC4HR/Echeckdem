using Echeckdem.Models;
using Microsoft.AspNetCore.Mvc;
using Echeckdem.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Echeckdem.CustomFolder;
using Microsoft.EntityFrameworkCore;



namespace Echeckdem.Controllers
{
    public class TrackScopeSetupController : Controller
    {

        private readonly DbEcheckContext _EcheckContext;

        public TrackScopeSetupController(DbEcheckContext EcheckContext)

        {
            _EcheckContext = EcheckContext;
        }

        public ActionResult Index()
        {
            var trackScopes = _EcheckContext.TrackScopes
                .Join(_EcheckContext.BocwScopes,
                    ts => ts.ScopeId,
                    bs => bs.ScopeId,
                    (ts, bs) => new { ts, bs })
                .Join(_EcheckContext.Maststates,
                    tsbs => tsbs.ts.Stateid,
                    ms => ms.Stateid,
                    (tsbs, ms) => new TrackScopeViewModel
                    {
                        WorkId = tsbs.ts.WorkId,
                        Task = tsbs.ts.Task,
                        Reminder = tsbs.ts.Reminder,
                        FirstAlert = tsbs.ts.FirstAlert,
                        ScopeName = tsbs.bs.ScopeName,
                        StateName = ms.Statedesc,
                        DueMonth = tsbs.ts.DueMonth,
                        DueDate = tsbs.ts.DueDate
                    })
                .ToList();

            return View(trackScopes); // No need to specify "Index" if the view name matches the action.
        }


        public ActionResult Create()
        {
            ViewBag.ScopeList = new SelectList(_EcheckContext.BocwScopes.ToList(), "ScopeId", "ScopeName");
            ViewBag.StateList = new SelectList(_EcheckContext.Maststates.ToList(), "Stateid", "Statedesc");
            return PartialView();
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

            ViewBag.ScopeList = new SelectList(_EcheckContext.BocwScopes.ToList(), "ScopeId", "ScopeName", trackScope.ScopeId);
            ViewBag.StateList = new SelectList(_EcheckContext.Maststates.ToList(), "Stateid", "Statedesc", trackScope.Stateid);

            return View(trackScope);
        }

        public ActionResult Edit(int WorkId)
        {
         
            var trackScope = _EcheckContext.TrackScopes.FirstOrDefault(w => w.WorkId == WorkId);
            if (trackScope == null)
            {
                return NotFound();
            }

            var viewModel = new TrackScopeViewModel
            {
                WorkId = trackScope.WorkId,
                Task = trackScope.Task,
                Reminder = trackScope.Reminder,
                FirstAlert = trackScope.FirstAlert,
                ScopeName = _EcheckContext.BocwScopes.FirstOrDefault(s => s.ScopeId == trackScope.ScopeId)?.ScopeName,
                StateName = _EcheckContext.Maststates.FirstOrDefault(s => s.Stateid == trackScope.Stateid)?.Statedesc,
                DueMonth = trackScope.DueMonth,
                DueDate = trackScope.DueDate
            };

            ViewBag.ScopeList = new SelectList(_EcheckContext.BocwScopes.ToList(), "ScopeId", "ScopeName", trackScope.ScopeId);
            ViewBag.StateList = new SelectList(_EcheckContext.Maststates.ToList(), "Stateid", "Statedesc", trackScope.Stateid);
            return PartialView(viewModel);
        }

        [HttpPost]
      
        public ActionResult Edit(TrackScope trackScope)
        {
            if (ModelState.IsValid)
            {
                _EcheckContext.TrackScopes.Update(trackScope);
                _EcheckContext.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ScopeList = new SelectList(_EcheckContext.BocwScopes.ToList(), "ScopeId", "ScopeName", trackScope.ScopeId);
            ViewBag.StateList = new SelectList(_EcheckContext.Maststates.ToList(), "Stateid", "Statedesc", trackScope.Stateid);
            return PartialView(trackScope);
        }

    }
}
