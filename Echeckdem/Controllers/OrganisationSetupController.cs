using OfficeOpenXml;
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
            return PartialView("bulkupload");
            
        }

        [HttpPost]

        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                //ViewBag.Message = "Please upload a valid Excel file.";
                //return View("bulkupload");
                return Json(new { success = false, message = "Please upload a valid Excel file." });
            }

            var recordCount = await _bulkUploadService.UploadLocationDataAsync(file);
            return Json(new { success = true, message = $"{recordCount} records uploaded successfully." });
        }

        public IActionResult DownloadExcelFile()
        {
            // Set EPPlus license context for .NET Core
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Create a new Excel package
            using (var package = new ExcelPackage())
            {
                // Add a worksheet
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Add headers to the worksheet
                worksheet.Cells[1, 1].Value = "Lcode";
                worksheet.Cells[1, 2].Value = "Oid";
                worksheet.Cells[1, 3].Value = "Lname";
                worksheet.Cells[1, 4].Value = "Lcity";
                worksheet.Cells[1, 5].Value = "Lstate";
                worksheet.Cells[1, 6].Value = "Lregion";


                // Generate file content
                var fileContent = package.GetAsByteArray();

                // Return file as download
                return File(fileContent,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Location Template.xlsx");
            }
        }

}
}








