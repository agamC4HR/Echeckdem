using DocumentFormat.OpenXml.Math;
using Echeckdem.CustomFolder;
using Echeckdem.Handlers;
using Echeckdem.Models;
using Echeckdem.Services;
using Echeckdem.ViewModel;
using Echeckdem.ViewModel.OnGoingActivity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography;
using System.Text.Json;
namespace Echeckdem.Controllers
{
    public class TrackerController : Controller
    {
        private readonly TrackerService _trackerService;
        private readonly IUserService _userService;
        private readonly DbEcheckContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFilter _filter;
        public TrackerController(TrackerService trackerService, IUserService userService, DbEcheckContext context, IWebHostEnvironment webHostEnvironment,IFilter filter)
        {
            _trackerService = trackerService;
            _userService = userService;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _filter = filter;
        }

      
        public IActionResult TrackerList()
        {           
            var fullActions = _trackerService.GetNcActionsForUser();
            return View(fullActions); 
        }

        public async Task<IActionResult> TrackerDet(string Acid ) 
        {
            var ncActions = await _trackerService.GetNcActionDet(Acid);

            return View(ncActions);
        }

        public async Task<IActionResult> Index()
        {

            var _UserlocationList = JsonSerializer.Deserialize<List<UserLocation>>(HttpContext.Session.GetString("Userlocation"));
            var model = new NewActivityViewModel
            {
                Organizations =  _UserlocationList.Select(x => new SelectListItem { Value = x.Oid, Text = x.Client }).DistinctBy(x => x.Value).ToList(),
                TPPDropdown = _trackerService.GetTPPDropdown(),
                ActDropdown = _trackerService.GetActDropdown(),
                //SlaDropdown = _trackerService.GetSlaDropdown(),
                ACTPDropdown = _trackerService.GetACTPDropdown(),
                Locations = _UserlocationList.Select(x => new SelectListItem { Value = x.Lcode, Text = x.Site}).DistinctBy(x => x.Value).ToList(),

            };

            return View(model); 
        }
        public IActionResult GetLocationsByOid(string oid)
        {
           return Json(_filter.GetLocationsByOid(oid));
        }

        [HttpPost]
        public IActionResult Create(NewActivityViewModel model)
        {
            var _UserlocationBOList = JsonSerializer.Deserialize<List<UserLocation>>(HttpContext.Session.GetString("Userbolocation"));
            if (!ModelState.IsValid)
            {
                
                model.Organizations = _UserlocationBOList.Select(x => new SelectListItem { Value = x.Oid, Text = x.Client }).DistinctBy(x => x.Value).ToList();
                model.TPPDropdown = _trackerService.GetTPPDropdown();
                model.ActDropdown = _trackerService.GetActDropdown();
                //model.SlaDropdown = _trackerService.GetSlaDropdown();
                model.ACTPDropdown = _trackerService.GetACTPDropdown();
                model.Locations = _UserlocationBOList.Select(x => new SelectListItem { Value = x.Lcode, Text = x.Site }).DistinctBy(x => x.Value).ToList();

                return View("Index", model);
            }

            try
            {
                Ncaction Activity = new Ncaction();
                Activity.Oid = model.SelectedOid;
                Activity.Lcode = model.SelectedLCODE;
                Activity.Actp = model.SelectedACTP;
                Activity.Actitle = _trackerService.GetActivityName(model.SelectedACTP)+" for "+ _trackerService.GetActName(model.SelectedACTITLE) ;
                Activity.Acstatus = "O";
                Activity.Acstatus = "N";
                Activity.Acshow = 1;
                Activity.Acruser = HttpContext.Session.GetInt32("UNO");
                Activity.Acrdate = DateOnly.FromDateTime(DateTime.Today);

                _context.Ncactions.Add(Activity);
                _context.SaveChanges();

                return RedirectToAction("TrackerDet", new { Acid=Activity.Acid.ToString()});
            }
            catch (Exception ex)
            {
                
                ModelState.AddModelError("", "An error occurred while saving data.");
                model.Organizations = _UserlocationBOList.Select(x => new SelectListItem { Value = x.Oid, Text = x.Client }).DistinctBy(x => x.Value).ToList();
                model.Locations = _UserlocationBOList.Select(x => new SelectListItem { Value = x.Lcode, Text = x.Site }).DistinctBy(x => x.Value).ToList();
                return View("Index", model); // Return to form with error message
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveNcAction(TrackerFullViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            

            try
            {
                var existingAction = _context.Ncactions.FirstOrDefault(a => a.Acid == model.Acid);
                if (existingAction == null)
                    return NotFound();

                existingAction.Actitle = model.Title;
                existingAction.Acstatus = model.Acstatus;
                
                existingAction.Acistatus = model.Acistatus;
                existingAction.Acdetail = model.Acdetail;
                existingAction.Acidate = model.Acidate;
                existingAction.Adocdate = model.Adocdate;
                existingAction.Accldate = model.Accldate;
                existingAction.Acremarks = model.Remark;
                if (model.UploadedFile != null && model.UploadedFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "files", model.Oid, "Acts");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);
                    var fileName = Path.GetFileName(model.UploadedFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.UploadedFile.CopyToAsync(stream);
                    }
                    var ncFile = new Ncfile
                    {
                        Oid = model.Oid,
                        Ftp = "Act",
                        Flink = existingAction.Acid,
                        Fname = fileName,
                        Fupdate = DateOnly.FromDateTime(DateTime.Today)
                    };

                    _context.Ncfiles.Add(ncFile);
                }
                _context.Ncactions.Update(existingAction);
                await _context.SaveChangesAsync();

                return RedirectToAction("TrackerDet", new { Acid = model.Acid.ToString() });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error: " + ex.Message);
            }
        }

       [HttpPost]
        public async Task<IActionResult> SaveNcActTaken(AddNcactaken model)
        {

            var uno = HttpContext.Session.GetInt32("UNO");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                var takenModel = new Ncactaken
                {
                    Acid = model.Acid,
                    Acdate = model.Acdate,
                    Actaken = model.Actaken,
                    Nacdate = model.Nacdate,
                    Accrdate = DateOnly.FromDateTime(DateTime.Today),
                    Showclient = 1,
                    Uno = uno
                };
                _context.Ncactakens.Add(takenModel);
                await _context.SaveChangesAsync();
                return RedirectToAction("TrackerDet", new { Acid = model.Acid.ToString() });
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Error: " + ex.Message);
            }
        }


    }
}
