using Echeckdem.CustomFolder;
using Echeckdem.Handlers;
using Echeckdem.Models;
using Microsoft.AspNetCore.Mvc;

namespace Echeckdem.Controllers
{
    public class StateTemplateController : Controller
    {
        private readonly IStateTemplateService _templateService;
        private readonly DbEcheckContext _dbEcheckContext;
        public StateTemplateController(IStateTemplateService templateService,DbEcheckContext dbEcheckContext)
        {
            _templateService = templateService;
            _dbEcheckContext = dbEcheckContext;
        }

        public async Task<IActionResult> Index()
        {
            var (states, counts, countsRet) = await _templateService.GetAllStatesAsync();
            ViewBag.ContributionCounts = counts;
            ViewBag.ReturnCounts = countsRet;
            return View(states);
        }

        public async Task<IActionResult> Details(string id)
        {
            var templates = await _templateService.GetTemplatesByStateAsync(id);
            ViewBag.StateId = id;
 
            var stateName = (from k in _dbEcheckContext.Maststates
                             where k.Stateid.Trim() == id.Trim()
                             select k.Statedesc).FirstOrDefault();

            ViewBag.StateName = stateName;
            return View(templates);
        }

        public async Task<IActionResult> Edit(int? id, string stateId)
        {
            if (id == null)
            {
             
              var StateName = (from k in _dbEcheckContext.Maststates where k.Stateid.Trim() == stateId.Trim() select k.Statedesc).FirstOrDefault();
                return View(new StateTemplateViewModel { CState = stateId, StateName=StateName, Cid=0 });
            }

            var template = await _templateService.GetTemplateByIdAsync(id.Value);

         

            return View(template);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(StateTemplateViewModel entry)
        {
            if (ModelState.IsValid)
            {
                await _templateService.AddOrUpdateTemplateAsync(entry);
                return RedirectToAction("Details", new { id = entry.CState });
            }

            return View(entry);
        }

        public async Task<IActionResult> Delete(int id, string stateId)
        {
            await _templateService.DeleteTemplateAsync(id);
            return RedirectToAction("Details", new { id = stateId });
        }

        //----------------------------------START-------------------------------RETURNS-------------------------------------------------------------//

        public async Task<IActionResult> DetailsRet(string id)
        {
            var templates = await _templateService.GetTemplateRetByStateAsync(id);
            ViewBag.StateId = id;

            var stateName = (from k in _dbEcheckContext.Maststates
                             where k.Stateid.Trim() == id.Trim()
                             select k.Statedesc).FirstOrDefault();

            ViewBag.StateName = stateName;
            return View(templates);
        }

        public async Task<IActionResult> EditRet(int? id, string stateId)
            {
            if (id == null)
            {
                var StateName = (from k in _dbEcheckContext.Maststates where k.Stateid.Trim() == stateId.Trim() select k.Statedesc).FirstOrDefault();

                return View(new StateTemplateRetViewModel { Rstate = stateId, StateName = StateName, Rcode = 0 });
            }

            var template = await _templateService.GetTemplateRetByIdAsync(id.Value);

            return View(template); 
        }

        [HttpPost]
        public async Task<IActionResult> EditRet(StateTemplateRetViewModel entry)
        {
            if (ModelState.IsValid)
            {
                await _templateService.AddOrUpdateTemplateRetAsync(entry);
                //return Ok();
               return RedirectToAction("DetailsRet", new { id = entry.Rstate });
            }

            return View(entry);
        }

        public async Task<IActionResult> DeleteRet(int id, string stateId)
        {
            await _templateService.DeleteTemplateRetAsync(id);
            return RedirectToAction("DetailsRet", new { id = stateId });
        }

        //----------------------------------END----------------------------------RETURNS-------------------------------------------------------------//
    }
}
