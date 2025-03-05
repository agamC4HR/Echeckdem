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

        //public ActionResult Index()
        //{
        //    var trackScopes = _EcheckContext.TrackScopes
        //        .Join(_EcheckContext.BocwScopes,
        //            ts => ts.ScopeId,
        //            bs => bs.ScopeId,
        //            (ts, bs) => new { ts, bs })
        //        .Join(_EcheckContext.Maststates,
        //            tsbs => tsbs.ts.Stateid,
        //            ms => ms.Stateid,
        //            (tsbs, ms) => new
        //            {
        //                tsbs.ts.WorkId,
        //                tsbs.ts.Task,
        //                tsbs.ts.Reminder,
        //                tsbs.ts.FirstAlert,
        //                ScopeName = tsbs.bs.ScopeName,
        //                StateName = ms.Statedesc
        //            })
        //        .ToList();

        //    return View("Index", trackScopes);
        //}


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
                        StateName = ms.Statedesc
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

    }
}
