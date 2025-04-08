using Echeckdem.CustomFolder;
using Echeckdem.Services;
using Microsoft.AspNetCore.Mvc;

namespace Echeckdem.Controllers
{
    public class StateTemplateController : Controller
    {
        private readonly IStateTemplateService _templateService;

        public StateTemplateController(IStateTemplateService templateService)
        {
            _templateService = templateService;
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
            return View(templates);
        }

        public async Task<IActionResult> Edit(int? id, string stateId)
        {
            if (id == null)
            {
                return View(new StateTemplateViewModel { CState = stateId });
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
            return View(templates);
        }

        public async Task<IActionResult> EditRet(int? id, string stateId)
        {
            if (id == null)
            {
                return PartialView("_EditRetPartial", new StateTemplateRetViewModel { Rstate = stateId });
            }

            var template = await _templateService.GetTemplateRetByIdAsync(id.Value);

            return PartialView("_EditRetPartial", template); // return View(template);
        }           

        [HttpPost]
        public async Task<IActionResult> EditRet(StateTemplateRetViewModel entry)
        {
            if (ModelState.IsValid)
            {
                await _templateService.AddOrUpdateTemplateRetAsync(entry);
                return Ok();
               // return RedirectToAction("DetailsRet", new { id = entry.Rstate });
            }

            return PartialView("_EditRetPartial", entry);
        }

        public async Task<IActionResult> DeleteRet(int id, string stateId)
        {
            await _templateService.DeleteTemplateRetAsync(id);
            return RedirectToAction("DetailsRet", new { id = stateId });
        }

        //----------------------------------END----------------------------------RETURNS-------------------------------------------------------------//
    }
}
