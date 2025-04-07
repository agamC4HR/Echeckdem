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
            var states = await _templateService.GetAllStatesAsync();
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

    }
}
