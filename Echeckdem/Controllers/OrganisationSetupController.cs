using OfficeOpenXml;
using Microsoft.AspNetCore.Mvc;
using Echeckdem.Services;
using Echeckdem.CustomFolder;
using System.Security.Cryptography;

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
        public IActionResult AddOrganisation()                    // Add Organisation details
        {
            return PartialView("AddOrganisation");
        }

        [HttpPost]                                                // Add Organisation details 
        public async Task<IActionResult> AddOrganisation(OrganisationGeneralInfoViewModel newOrganisation)
        {
            if (ModelState.IsValid)
            {
                var isAdded = await _organisationsetupservice.AddOrganisationAsync(newOrganisation);

                if (isAdded)
                {
                    TempData["SuccessMessage"] = "Organisation added successfully.";
                    return RedirectToAction("OrganisationSetup");
                }
            }
            return RedirectToAction("OrganisationSetup");
        }


        [HttpGet]                            // Getting Organisation List and General Info
        public async Task<IActionResult> OrganisationSetup(string? searchTerm, string? selectedOid)
        {
            var viewModel = await _organisationsetupservice.GetOrganisationSetupAsync(searchTerm, selectedOid);

            return View("OrganisationSetup", viewModel);
        }

        [HttpPost]                                               // Editing/Updating Organisation General Info 
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

        // Add Locations process        --  1)  BULK  UPLOAD 
        [HttpGet]
        public IActionResult Upload()
        {
            return PartialView("bulkupload");
            
            
        }

        [HttpPost]                                                          // Add Locations process        --  1)  BULK UPLOAD
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return Json(new { success = false, message = "*Error: Please upload a valid Excel file." });
            }

            var recordCount = await _bulkUploadService.UploadLocationDataAsync(file);
            return Json(new { success = true, message = $"{recordCount} records uploaded successfully." });
        }

        public IActionResult DownloadExcelFile()                             // Downloading excel tempelate file 
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




        // geting locations data for ADDLOCATIONS Button
        //[HttpGet]
        //public async Task<IActionResult> GetLocationDatabyOid(string oid)

        //{
        //    var locations = await _organisationsetupservice.GetLocationDatabyOidAsync(oid);
        //        if(locations == null || !locations.Any())
        //    {
        //        TempData["ErrorMessage"] = $"No locations found for OID: {oid}";
        //        return RedirectToAction("OrganisationSetup");
        //    }

        //    return View("EditLocations", locations);
        //}
        //[HttpGet]
        //public async Task<IActionResult> GetLocationDatabyOid(string oid)
        //{
        //    var locations = await _organisationsetupservice.GetLocationDatabyOidAsync(oid);

        //    if (locations == null || !locations.Any())
        //    {
        //        TempData["ErrorMessage"] = $"No locations found for OID: {oid}";
        //        return RedirectToAction("OrganisationSetup");
        //    }

        //    // Create an instance of CombinedOrganisationSetupViewModel
        //    var model = new CombinedOrganisationSetupViewModel
        //    {
        //        AddLocation = locations, // Assuming locations is of type List<AddLocationViewModel>
        //        oid = oid // Set the OID if needed
        //    };

        //    return PartialView("EditLocations", model);
        //}

        //[HttpPut]

        //public async Task<IActionResult> UpdateLocations([FromForm] List<AddLocationViewModel> updatedlocationdata)
        //{
        //    if (updatedlocationdata == null || !updatedlocationdata.Any())
        //    {
        //        TempData["ErrorMessage"] = "No location data provided.";
        //        return PartialView("EditLocations", updatedlocationdata);
        //    }

        //    try
        //    {
        //        // Update locations data in the database
        //        bool result = await _organisationsetupservice.AddLocationDataAsync(updatedlocationdata);

        //        if (result)
        //        {
        //            TempData["SuccessMessage"] = "Locations updated successfully.";
        //            // Redirect to the GetLocationDatabyOid action after update
        //            return RedirectToAction("GetLocationDatabyOid", new { oid = updatedlocationdata.FirstOrDefault()?.Oid });
        //        }
        //        else
        //        {
        //            TempData["ErrorMessage"] = "Failed to update locations.";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ErrorMessage"] = $"An error occurred while updating locations: {ex.Message}";
        //        Console.WriteLine($"Update error: {ex}");
        //        Console.WriteLine($"Stack Trace: {ex.StackTrace}");
        //    }

        //    return PartialView("EditLocations", updatedlocationdata);
        //}
        [HttpGet]
        public IActionResult GetLocationDatabyOid(string oid)
        {
            var locations = _organisationsetupservice.GetLocationDatabyOidAsync(oid).Result;

            if (string.IsNullOrEmpty(oid))
            {
                TempData["ErrorMessage"] = "Invalid OID.";
                return RedirectToAction("OrganisationSetup");
            }

            //var locations = await _organisationsetupservice.GetLocationDatabyOidAsync(oid);
            
            //if (locations == null || !locations.Any())
            //if (!locations.Any())
            //{
            //    TempData["ErrorMessage"] = $"No locations found for OID: {oid}";
            //    return RedirectToAction("OrganisationSetup");
            //}

            // Create an instance of CombinedOrganisationSetupViewModel
            var model =  new CombinedOrganisationSetupViewModel
            {
                AddLocation = locations, // Assuming locations is of type List<AddLocationViewModel>
                oid = oid // Set the OID if needed
            };

            return PartialView("EditLocations", model);
        }



        [HttpPost]
        public async Task<IActionResult> UpdateLocations(CombinedOrganisationSetupViewModel updatedLocationData)
        {
            //if (updatedLocationData == null || updatedLocationData.AddLocation == null || !updatedLocationData.AddLocation.Any())
            //{
            //    TempData["ErrorMessage"] = "No location data provided.";
            //    return PartialView("EditLocations", updatedLocationData);
            //}

            if (updatedLocationData.Lcode == null)
            {
                TempData["ErrorMessage"] = "No data received.";
                return PartialView("EditLocations", new CombinedOrganisationSetupViewModel());
                
            }

            //if (string.IsNullOrEmpty(updatedLocationData.Oid))
            //{
            //    TempData["ErrorMessage"] = "Oid is missing.";
            //    return PartialView("EditLocations", updatedLocationData);
            //}

            try
            {
                // Update locations data in the database
                bool result = await _organisationsetupservice.AddLocationDataAsync(updatedLocationData);

                if (result)
                {
                    TempData["SuccessMessage"] = "Locations updated successfully.";
                    // Redirect to the GetLocationDatabyOid action after update
                    return RedirectToAction("GetLocationDatabyOid", new { oid = updatedLocationData.oid });
                
                }
                
                else
                {
                    TempData["ErrorMessage"] = "Failed to update locations.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while updating locations: {ex.Message}";
                Console.WriteLine($"Update error: {ex}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
               
            }

            // If something fails, return the same model to the view
            return PartialView("EditLocations", updatedLocationData);
        }



       



    }
}








