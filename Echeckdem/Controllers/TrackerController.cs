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

        public IActionResult TrackerList()
        {
            int uno = _trackerService.GetUnoFromSession(HttpContext);
            var fullActions = _trackerService.GetFullNcActionsForUser(uno);
            return View(fullActions); 
        }

        public async Task<IActionResult> Index()
        {
           
            int uno = _trackerService.GetUnoFromSession(HttpContext);


            var model = new TrackerViewModel
            {
                Organizations = _trackerService.GetOrganizations(uno),
                TPPDropdown = _trackerService.GetTPPDropdown(),
                ActDropdown = _trackerService.GetActDropdown(),
                SlaDropdown = _trackerService.GetSlaDropdown(),
                Locations = new List<SelectListItem>() 
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
         
            int uno = _trackerService.GetUnoFromSession(HttpContext);

            var locations = _trackerService.GetLocations(uno, oid);
            return Json(locations);
        }

        [HttpPost]
        public IActionResult Create(TrackerViewModel model)
        {
            foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(modelError.ErrorMessage); // Log errors
            }

            if (!ModelState.IsValid) 
            {
                model.SelectedUno = _trackerService.GetUnoFromSession(HttpContext);
                model.Organizations = _trackerService.GetOrganizations(model.SelectedUno);
                model.Locations = _trackerService.GetLocations(model.SelectedUno, model.SelectedOid);
                model.TPPDropdown = _trackerService.GetTPPDropdown();
                model.ActDropdown = _trackerService.GetActDropdown();
                model.SlaDropdown = _trackerService.GetSlaDropdown();
                return View("Index", model); 
            }
            
            try
            {
                _trackerService.SaveNcAction(model);
                return RedirectToAction("TrackerList");
            }
            catch (Exception ex)
            {                
                ModelState.AddModelError("", "An error occurred while saving data.");
                model.Organizations = _trackerService.GetOrganizations(model.SelectedUno);
                model.Locations = _trackerService.GetLocations(model.SelectedUno, model.SelectedOid);
                return View("Index", model); // Return to form with error message
            }
        }

        [HttpGet]
        public IActionResult EditNcAction(int acid)
        {
            var ncAction = _trackerService.GetNcActionById(acid);
            return ncAction == null ? NotFound() : Json(ncAction);
        }


        [HttpPost]
        public IActionResult SaveNcAction(TrackerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _trackerService.SaveOrUpdateNcAction(model);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult UploadNcFile(int acid, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            try
            {
                _trackerService.SaveNcFile(acid, file);
                return Json(new { success = true, message = "File uploaded successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }


        [HttpGet]
        public IActionResult EditNcActTaken(int acid)
        {
            var actionTaken = _trackerService.GetNcActTakenByAcid(acid);

            return Json(actionTaken);
        }

        [HttpPost]
        public IActionResult SaveNcActTaken(TrackerFullViewModel model)
        {
            
            var uno = HttpContext.Session.GetInt32("UNO");

            
            if (!uno.HasValue)
            {
                return BadRequest("UNO is missing in session.");
            }

           // modelstate uno mei dikkat hai
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                
                var takenModel = new TrackerTakenViewModel
                {
                    Acid = model.Acid,        
                    Acdate = model.Acdate,    
                    Actaken = model.Actaken, 
                    Nacdate = model.Nacdate, 
                    Showclient = model.Showclient,
                    Uno = uno.Value           
                };

                
                _trackerService.SaveOrUpdateNcActTaken(takenModel);

                
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, "Error: " + ex.Message);
            }
        }

     


    }
}
