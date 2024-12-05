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

        public OrganisationSetupController(OrganisationSetupService organisationSetupService, IBulkUploadService bulkUploadService)

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

        [HttpPost]
        public async Task<IActionResult> EditOrganisationInfo(OrganisationGeneralInfoViewModel updatedInfo)
        {
            var isUpdated = await _organisationsetupservice.UpdateOrganisationInfoAsync(updatedInfo);

            if (isUpdated)
            {
                TempData["SuccessMessage"] = "Record updated successfully.";
                return RedirectToAction("OrganisationSetup", new { selectedOid = updatedInfo.oid });

            }

            return BadRequest("Failed to update organization information");
        }

        // Add Locations process        --  1)  BULK UPLOAD
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
                return Json(new { success = false, message = "*Error: Please upload a valid Excel file." });
            }

            var recordCount = await _bulkUploadService.UploadLocationDataAsync(file);
            return Json(new { success = true, message = $"{recordCount} records uploaded successfully." });
        }


        //Add Organisation------------------------

        [HttpGet]
        public IActionResult AddOrganisation()
        {
            return PartialView("AddOrganisation");
        }

        [HttpPost]

        public async Task<IActionResult> AddOrganisation(OrganisationGeneralInfoViewModel newOrganisation)
        {
            if (!ModelState.IsValid)
            {
                TempData["Status"] = "error";
                TempData["Message"] = "Validation failed.";
                return RedirectToAction("OrganisationSetup");
            }

            try
            {
                var isAdded = await _organisationsetupservice.AddOrganisationAsync(newOrganisation);

                if (isAdded)
                {
                    TempData["Status"] = "success";
                    TempData["Message"] = "Organisation added successfully.";
                }
                else
                {
                    TempData["Status"] = "error";
                    TempData["Message"] = "Failed to add organisation.";
                }
            }
            catch (Exception ex)
            {
                TempData["Status"] = "error";
                TempData["Message"] = "An error occurred: " + ex.Message;
            }

            return RedirectToAction("OrganisationSetup");
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








