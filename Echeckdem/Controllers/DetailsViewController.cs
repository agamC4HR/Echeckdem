using Echeckdem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using Echeckdem.Services;
using System.Runtime.CompilerServices;
using Echeckdem.CustomFolder;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using System.Text.Json;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Security.Cryptography;
using Echeckdem.ViewModel;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc.Rendering;
using Echeckdem.ViewModel.Shared;
using DocumentFormat.OpenXml.InkML;
using Echeckdem.ViewModel.ProjectBocw;
using Echeckdem.ViewModel.ComplianceTracker;
using Echeckdem.Handlers;

namespace Echeckdem.Controllers
{
    public class DetailsViewController : Controller
    {   
        private readonly RegistrationService _regService;
        private readonly ContributionService _contService;
        private readonly ReturnsService _retService;
        
        private readonly ProjectBocwService _bocwService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly DbEcheckContext _context;
        private readonly IFilter _filter;
        public DetailsViewController(RegistrationService regService, ContributionService contService, ReturnsService retService, IWebHostEnvironment webHostEnvironment, ProjectBocwService bocwService, DbEcheckContext dbEcheckContext, IFilter filter    )
        {
            _regService = regService;
            _contService = contService;
            _retService = retService;
            _bocwService = bocwService;
            _webHostEnvironment = webHostEnvironment;
            _context = dbEcheckContext;
            _filter = filter;
        }
        [HttpGet]
        public async Task<IActionResult> Index() 
        {
            CombinedDetailedViewModel model = new CombinedDetailedViewModel();
            

            
            model.Registrations= await _regService.GetRegistrationAsync();
            model.Contributions = await _contService.GetContributionAsync();
            model.Returns = await _retService.GetReturnAsync();
            model.BOCW = await _bocwService.GetBocwAsync();
            var _UserlocationList = JsonSerializer.Deserialize<List<UserLocation>>(HttpContext.Session.GetString("Userlocation"));
            model.FilterFormModel = new FilterFormModel();
            model.FilterFormModel.UClientList = _UserlocationList.Select(x => new SelectListItem { Value = x.Oid, Text = x.Client }).DistinctBy(x => x.Value).ToList();
            model.FilterFormModel.USiteList = _UserlocationList.Select(x => new SelectListItem { Value = x.Lcode, Text = x.Site }).DistinctBy(x => x.Value).ToList();
            model.FilterFormModel.UStateList = _UserlocationList.Select(x => new SelectListItem { Value = x.Lstate, Text = x.Lstate }).DistinctBy(x => x.Value).ToList();
            model.FilterFormModel.UCityList = _UserlocationList.Where(x=>!string.IsNullOrEmpty(x.Lcity)).Select(x => new SelectListItem { Value = x.Lcity, Text = x.Lcity }).DistinctBy(x => x.Value).ToList();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> FilteredIndex(FilterFormModel filter)
        {
            if(!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest("Invalid filter data."+filter+"<br/>"+errors  );
            }
            CombinedDetailedViewModel model = new CombinedDetailedViewModel();

            model.FilterFormModel = filter;
            
            var _UserlocationList = JsonSerializer.Deserialize<List<UserLocation>>(HttpContext.Session.GetString("Userlocation"));
          
            model.FilterFormModel.UClientList = _UserlocationList.Select(x => new SelectListItem { Value = x.Oid, Text = x.Client }).DistinctBy(x => x.Value).ToList();
            model.FilterFormModel.USiteList = _UserlocationList.Select(x => new SelectListItem { Value = x.Lcode, Text = x.Site }).DistinctBy(x => x.Value).ToList();
            model.FilterFormModel.UStateList = _UserlocationList.Select(x => new SelectListItem { Value = x.Lstate, Text = x.Lstate }).DistinctBy(x => x.Value).ToList();
            model.FilterFormModel.UCityList = _UserlocationList.Where(x => !string.IsNullOrEmpty(x.Lcity)).Select(x => new SelectListItem { Value = x.Lcity, Text = x.Lcity }).DistinctBy(x => x.Value).ToList();
            model.Registrations = await _regService.GetRegistrationAsync(filter);
            model.Contributions = await _contService.GetContributionAsync(filter);
            model.Returns = await _retService.GetReturnAsync(filter);
            model.BOCW = await _bocwService.GetBocwAsync(filter);


            return View("Index",model);
        }
        public IActionResult GetLocationsByOid(string oid)
        {
            return Json(_filter.GetLocationsByOid(oid));
        }
        public IActionResult GetCityByOid(string oid)
        {
            return Json(_filter.GetCityByOid(oid));
        }
        public IActionResult GetStateByOid(string oid)
        {
            return Json(_filter.GetStateByOid(oid));
        }
        public IActionResult GetLocations()
        {
            return Json(_filter.GetLocations());
        }
        public IActionResult GetCity()
        {
            return Json(_filter.GetCity());
        }
        public IActionResult GetState()
        {
            return Json(_filter.GetState());
        }

        //----------------START----------------------------------REGISTRATION--------  ----------------------------------------------------------------------//

        public async Task<IActionResult> EditReg(int uid, string oid, string lcode)
        {
            var ncreg = await _regService.GetByIdAsync(uid, oid, lcode);
            if (ncreg == null)
            {
                return NotFound();
            }

            var registrationViewModel = new RegistrationViewModel
            {
                Uid = ncreg.Uid,
                Oid = ncreg.Oid,
                Lcode = ncreg.Lcode,
                Status = ncreg.Status,
                Rno = ncreg.Rno,
                Noe = ncreg.Noe,
                Nmoe = ncreg.Nmoe,
                Doi = ncreg.Doi,
                Doe = ncreg.Doe,
                Dolr = ncreg.Dolr,
                Remarks = ncreg.Remarks,
                Filename = ncreg.Filename,
                // Map other properties as needed
            };
            return View("~/Views/DetailedView/EditReg.cshtml", registrationViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdateReg(int? uid, string oid, string lcode, string status, string rno, int noe, string nmoe, DateOnly? doi, DateOnly? doe, DateOnly? dolr, string remarks, IFormFile file)
        {
            var result = await _regService.UpdateRegAsync(uid, oid, lcode, status, rno, noe, nmoe, doi, doe, dolr, remarks, file);

            if (result.StartsWith("Error") || result == "Record not found.")
            {
                return BadRequest(result);
            }

            return Ok(new { Message = result });
        }

        public IActionResult Open_file(string tp, string nm, string oid)
        {
            try
            {
                // Determine folder path
                string folderName = tp switch
                {
                    "REG" => Path.Combine("Files", oid, "REG"),
                    "CONTR" => Path.Combine("Files", oid, "CONTR"),  // Contributions folder
                    "RET" => Path.Combine("Files", oid, "RET"),
                    "BOCW" => Path.Combine("Files", oid, "BOCW"),

                    _ => throw new ArgumentException("Invalid file type.")
                };

                // Combine with wwwroot path
                string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, folderName, nm);

                if (!System.IO.File.Exists(fullPath))
                {
                    return NotFound("File not found.");
                }

                // Serve file with appropriate content type
                return PhysicalFile(fullPath, "application/pdf");
            }
            catch (Exception ex)
            {
                string errorData = $"{ex.Message}\n{ex.StackTrace}";
                var stream = new MemoryStream(Encoding.UTF8.GetBytes(errorData));
                return File(stream, "text/plain", "error_log.txt");
            }
        }

        //----------------END----------------------------------REGISTRATION------------------------------------------------------------------------------//

        //----------------START----------------------------------CONTRIBUTION ------------------------------------------------------------------------------//
        [HttpGet]
        public async Task<IActionResult> EditContr(int contid, string oid, string lcode)
        {
            var nccontr = await _contService.GetByIdAsync(contid, oid, lcode);
            if (nccontr == null)
            {
                return NotFound();
            }

            var contributionviewmodel = new ContributionViewModel
            {
                //Uid = ncreg.Uid,
                Contid = nccontr.Contid,
                oid = nccontr.Oid,
                Lcode = nccontr.Lcode,
                Status = nccontr.Status,
                LastDate = nccontr.Lastdate,
                Amount = nccontr.Amount,
                Chqno = nccontr.Chqno,
                chqdate = nccontr.Chqdate,
                Depdate = nccontr.Depdate,
                Remarks = nccontr.Remarks,
                Filename = nccontr.Filename,
                // Map other properties as needed
            };
            return View("~/Views/DetailedView/EditContr.cshtml", contributionviewmodel);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdateContr(int? contid, string oid, string lcode, int status, string amount, string chqno, DateOnly? chqdate, DateOnly? deptdate, DateOnly? lastdate, string remarks, IFormFile file)
        {
            var result = await _contService.UpdateContrAsync(contid, oid, lcode, status, amount, chqno, chqdate, deptdate, lastdate, remarks, file);

            if (result.StartsWith("Error") || result == "Record not found.")
            {
                return BadRequest(result);
            }

            return Ok(new { Message = result });
        }

        //----------------END----------------------------------CONTRIBUTION ------------------------------------------------------------------------------/
        //----------------START----------------------------------RETURNS ----------------------------------------------------------------------------------//
        [HttpGet]
        public async Task<IActionResult> EditRet(int rtid, string oid, string lcode)
        {
            var ncret = await _retService.GetByIdAsync(rtid, oid, lcode);
            if (ncret == null)
            {
                return NotFound();
            }

            var returnviewmodel = new ReturnsViewModel
            {
                Rtid = ncret.Rtid,
                oid = ncret.Oid,
                Lcode = ncret.Lcode,
                Status = ncret.Status,
               Depdate = ncret.Depdate,
               LastDate = ncret.Lastdate,
               Remarks = ncret.Remarks,
               Filename = ncret.Filename,
                 
            };
            return View("~/Views/DetailedView/EditRet.cshtml", returnviewmodel);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdateRet(int? rtid, string oid, string lcode, int status, DateOnly? deptdate, DateOnly? lastdate, string remarks, IFormFile file)
        {
            var result = await _retService.UpdateRetAsync(rtid, oid, lcode, status, deptdate, lastdate,  remarks, file);

            if (result.StartsWith("Error") || result == "Record not found.")
            {
                return BadRequest(result);
            }

            return Ok(new { Message = result });
        }

        //----------------END----------------------------------RETURNS------------------------------------------------------------------------------------//
        //----------------START----------------------------------BOCW------------------------------------------------------------------------------------//
        [HttpGet]
        public async Task<IActionResult> EditBocw(string transactionId)
        {
            var model = await _bocwService.GetEditData(transactionId);
            if (model == null) return NotFound();

            return View(model);


        }
        [HttpPost]
        public async Task<IActionResult> UpdateBocw(NcbocwUpdateModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var task = await _context.Ncbocws.FindAsync(model.TransactionId);
            if (task == null)
                return NotFound();

            
            task.Status = model.Status;
            task.CompletionDate = model.CompletionDate;
            var oid = _context.Ncmlocs.Where(x => x.Lcode == task.Lcode).Select(x => x.Oid).FirstOrDefault();
            if (model.UploadedFile != null && model.UploadedFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "files", oid.ToString(), "bocw");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Path.GetFileName(model.UploadedFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.UploadedFile.CopyToAsync(stream);
                }

                task.FileName = fileName;
            }

            _context.Update(task);
            await _context.SaveChangesAsync();

            return RedirectToAction("EditBocw", new { transactionId = model.TransactionId.ToString() });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateBONcaction(BOCWNcactionEdit model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var task = await _context.Ncactions.FindAsync(model.Acid);
            if (task == null)
                return NotFound();

            
            task.Acdetail = model.Acdetail;
            task.Acremarks = model.Acremarks;
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
                    Ftp="Act",
                    Flink = task.Acid,
                    Fname = fileName,
                    Fupdate = DateOnly.FromDateTime(DateTime.Today)
                };

                _context.Ncfiles.Add(ncFile);
                
            }

            _context.Update(task);
            await _context.SaveChangesAsync();







            return RedirectToAction("EditBocw", new { transactionId = model.TransactionId.ToString() });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateBONcactaken(BocwNcactaken model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var task = new Ncactaken();


            task.Acid = model.Acid;
            task.Acdate = model.Acdate;
            task.Actaken = model.Actaken;
            task.Nacdate = model.Nacdate;
            task.Accrdate = DateOnly.FromDateTime(DateTime.Today);
            task.Showclient = 1;
task.Uno= HttpContext.Session.GetInt32("UNO");
            _context.Ncactakens.Add(task);
            await _context.SaveChangesAsync();







            return RedirectToAction("EditBocw", new { transactionId = model.TransactionId.ToString() });
        }

        
        //----------------END----------------------------------BOCW------------------------------------------------------------------------------------//
    }
}