using Echeckdem.CustomFolder;
using Echeckdem.Models;
using Echeckdem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Echeckdem.Controllers
{
    public class TrackerController : Controller
    {
        private readonly TrackerService _trackerService;
        private readonly IUserService _userService;

        public TrackerController(TrackerService trackerService, IUserService userService)
        {
            _trackerService = trackerService;
            _userService = userService;
        }

        public int GetUnoFromSession(HttpContext httpContext)
        {
            return httpContext.Session.GetInt32("UNO") ?? 0;
        }

        public async Task<IActionResult> Index()
        {
           // string userId = User.Identity.Name; // Assuming user is authenticated
            int uno = _trackerService.GetUnoFromSession(HttpContext);


            var model = new TrackerViewModel
            {
                Organizations = _trackerService.GetOrganizations(uno),
                TPPDropdown = _trackerService.GetTPPDropdown(),
                ActDropdown = _trackerService.GetActDropdown(),
                SlaDropdown = _trackerService.GetSlaDropdown(),
                Locations = new List<SelectListItem>() // Ensure it's initialized
            };

            try
            {
                var locations = _trackerService.GetLocations(uno, model.SelectedOid);
                model.Locations = locations ?? new List<SelectListItem>(); // Ensure non-null list
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while fetching locations: {ex.Message}";
                return View(model);
            }
        }


        public async Task<JsonResult> GetLocations(string oid)
        {
            //string userId = User.Identity.Name;
            int uno = _trackerService.GetUnoFromSession(HttpContext);

            var locations = _trackerService.GetLocations(uno, oid);
            return Json(locations);
        }

        [HttpPost]
        public IActionResult Create(TrackerViewModel model)
        {
            if (!ModelState.IsValid) // ✅ Check if model is valid before saving
            {
                model.SelectedUno = _trackerService.GetUnoFromSession(HttpContext);
                model.Organizations = _trackerService.GetOrganizations(model.SelectedUno);
                model.Locations = _trackerService.GetLocations(model.SelectedUno, model.SelectedOid);
                model.TPPDropdown = _trackerService.GetTPPDropdown();
                model.ActDropdown = _trackerService.GetActDropdown();
                model.SlaDropdown = _trackerService.GetSlaDropdown();
                return View("Index", model); // ✅ Return to form with errors
            }

           

            try
            {
                _trackerService.SaveNcAction(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log or display error message
                ModelState.AddModelError("", "An error occurred while saving data.");
                model.Organizations = _trackerService.GetOrganizations(model.SelectedUno);
                model.Locations = _trackerService.GetLocations(model.SelectedUno, model.SelectedOid);
                //model.TPPDropdown = _trackerService.GetTPPDropdown();
                //model.ActDropdown = _trackerService.GetActDropdown();
                //model.SlaDropdown = _trackerService.GetSlaDropdown();
                return View("Index", model); // Return to form with error message
            }
        }



    }
}
