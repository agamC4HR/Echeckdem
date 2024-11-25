using Microsoft.AspNetCore.Mvc;
using Echeckdem.Services;
using Echeckdem.CustomFolder;

namespace Echeckdem.Controllers
{
    public class OrganisationSetupController : Controller
    {
       private readonly OrganisationSetupService _organisationsetupservice;
       private readonly IBulkUploadService _bulkUploadService;

        public OrganisationSetupController (OrganisationSetupService organisationSetupService, IBulkUploadService bulkUploadService)

        {
            _organisationsetupservice = organisationSetupService;
            _bulkUploadService = bulkUploadService;
        }

        [HttpGet]
        public async Task<IActionResult> OrganisationSetup(string? searchTerm, string? selectedOid)
        {
            var viewModel = await _organisationsetupservice.GetOrganisationSetupAsync(searchTerm, selectedOid);
            
            return View("OrganisationSetup", viewModel);
        }
       
        // Add Locations
        [HttpGet]
        public IActionResult Upload()
        {
            return View("BulkUpload");
        }

        [HttpPost]

        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("File", "Please upload a valid Excel file.");
                return View();
            }

            var recordCount = await _bulkUploadService.UploadLocationDataAsync(file); 
            ViewBag.Message = $"{recordCount} records uploaded successfully.";
            
            return View("BulkUpload", recordCount);

        }




    }
}








