using OfficeOpenXml;
using Microsoft.AspNetCore.Mvc;
using Echeckdem.Services;
using Echeckdem.CustomFolder;
using System.Security.Cryptography;
using Echeckdem.Models;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Echeckdem.Controllers

{
    public class OrganisationSetupController : Controller
    {
        private readonly OrganisationSetupService _organisationsetupservice;
        private readonly IBulkUploadService _bulkUploadService;
        private readonly DbEcheckContext _EcheckContext;
        private readonly ILogger<OrganisationSetupController> _logger;


        public OrganisationSetupController(OrganisationSetupService organisationSetupService, IBulkUploadService bulkUploadService, DbEcheckContext EcheckContext, ILogger<OrganisationSetupController> logger)

        {
            _organisationsetupservice = organisationSetupService;
            _bulkUploadService = bulkUploadService;
            _EcheckContext = EcheckContext;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult AddOrganisation()                                                                                 // Add Organisation details (setting up new organisation)
        {
            return PartialView("AddOrganisation");
        }

        [HttpPost]                                                                                                          // Add Organisation details (setting up new organisation)
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
            TempData["ErrorMessage"] = "Failed to add organisation. Please try again.";
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
        public IActionResult Upload(string oid)
        {
            ViewBag.SelectedOid = oid;
            return PartialView("bulkupload");
            
         }

        [HttpPost]                                                          // Add Locations process        --  1)  BULK UPLOAD
        public async Task<IActionResult> Upload(IFormFile file, string oid )
        {
            if (String.IsNullOrEmpty(oid))
            {
                return Json(new { success =  false, message = "*Error: Please select an organisation." });
            }
            if (file == null || file.Length == 0)
            {
                return Json(new { success = false, message = "*Error: Please upload a valid Excel file." });
            }
            
            var recordCount = await _bulkUploadService.UploadLocationDataAsync(file, oid);
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
                worksheet.Cells[1, 1].Value = "Site Name";
                worksheet.Cells[1, 2].Value = "Site City";
                worksheet.Cells[1, 3].Value = "Site State";
                worksheet.Cells[1, 4].Value = "Site Region";
                worksheet.Cells[1, 5].Value = "Site Act";
                worksheet.Cells[1, 6].Value = "Site Address";
                worksheet.Cells[1, 7].Value = "Site FM";
                worksheet.Cells[1, 8].Value = "Site FM Email";
                worksheet.Cells[1, 9].Value = "Site FM Contact Number";
                worksheet.Cells[1, 10].Value = "Site Escalation1";
                worksheet.Cells[1, 11].Value = "Under CentralGovt";
                worksheet.Cells[1, 12].Value = "Under CLRA";
                worksheet.Cells[1, 13].Value = "Setup Year";
                worksheet.Cells[1, 14].Value = "Site Active";
                // Generate file content
                var fileContent = package.GetAsByteArray();
                // Return file as download
                return File(fileContent,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Location Template.xlsx");    
            }
        }

        [HttpGet]
        public IActionResult GetLocationDatabyOid(string oid)                                 // Viewing the Location DATA tab
        {
          

            if (string.IsNullOrEmpty(oid))
            {
                TempData["ErrorMessage"] = "Invalid OID.";
                return RedirectToAction("OrganisationSetup");
            }







            var locations = _organisationsetupservice.GetLocationDatabyOidAsync(oid).Result;


            // Create an instance of CombinedOrganisationSetupViewModel 
            var model =  new CombinedOrganisationSetupViewModel
            {
                AddLocation = locations, // Assuming locations is of type List<AddLocationViewModel>
                oid = oid // Set the OID if needed
            };

            return PartialView("EditLocations", model);
        }



        [HttpPost]                                                                                                          // EDting the location tab button to make any change.   

        public async Task<IActionResult> UpdateLocations(CombinedOrganisationSetupViewModel updatedLocationData)
        {
            if (updatedLocationData.Lcode == null)
            {
                TempData["ErrorMessage"] = "No data received.";
                return PartialView("EditLocations", new CombinedOrganisationSetupViewModel());
            }

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











            //if (string.IsNullOrEmpty(updatedLocationData.oid))
            //{
            //    TempData["ErrorMessage"] = "No data received.";
            //    return PartialView("EditLocations", new CombinedOrganisationSetupViewModel());
            //}

            //try
            //{
            //    if (await _organisationsetupservice.IncompleteBODataAsync(updatedLocationData.oid))
            //    {
            //        TempData["ErrorMessage"] = "Additional information is required for BOCW sites. Please upload the required data before proceeding.";
            //        return RedirectToAction("BOCWSiteSetup", new { oid = updatedLocationData.oid });
            //    }

            //    bool result = await _organisationsetupservice.AddLocationDataAsync(updatedLocationData);

            //    if (result)
            //    {
            //        TempData["SuccessMessage"] = "Locations updated successfully.";
            //        return RedirectToAction("GetLocationDatabyOid", new { oid = updatedLocationData.oid });
            //    }

            //    TempData["ErrorMessage"] = "Failed to update locations.";
            //}
            //catch (Exception ex)
            //{
            //    TempData["ErrorMessage"] = $"An error occurred while updating locations: {ex.Message}";
            //    Console.WriteLine($"Update error: {ex}");
            //}










            // If something fails, return the same model to the view
            return PartialView("EditLocations", updatedLocationData);
        }


        public IActionResult BOCWSiteSetup(string oid)
        {
            ViewBag.Oid = oid;
            // You can use the OID to pre-populate any necessary data or pass it along to the view
            return PartialView("BOCWBulkUpload", new { oid }); //,
        }


        [HttpPost]
        public async Task<IActionResult> UploadBOCWSiteDetails(IFormFile file, string oid)
        {
            if (string.IsNullOrEmpty(oid))
            {
                return Json(new { success = false, message = "Please provide a valid OID." });
            }

            if (file == null || file.Length == 0)
            {
                return Json(new { success = false, message = "Please upload a valid Excel file." });
            }

            try
            {
            
                var boSites = await _organisationsetupservice.GetBoSitesAsync(oid); // Method to fetch BO sites
                if (!boSites.Any())
                {
                    return Json(new { success = false, message = "No BO sites found for the provided OID." });
                }

                var hasBOTypeSites = boSites.Any(site => site.Ltype == "BO");
                if (hasBOTypeSites)
                {
                    return Json(new { success = false, message = "You must upload additional data for sites under BOCW before proceeding with other actions." });
                }

                var recordCount = await _organisationsetupservice.UploadBOCWSiteDetailsAsync(file, oid);

                return Json(new { success = true, message = $"{recordCount} BO site records uploaded successfully." });

               
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        


        }

        [HttpGet]
        public async Task<IActionResult> DownloadBOCWExcelFile(string oid)                             // Downloading excel tempelate file 
        {
            if (string.IsNullOrEmpty(oid))
            {
                return Json(new { success = false, message = "Please provide a valid OID." });
            }

            //var boSites = await _organisationsetupservice.GetBoSitesAsync(oid);




            var boSites = await _EcheckContext.Ncmlocs.Where(n => n.Oid == oid && n.Ltype == "BO").Select(n => new { n.Lcode, n.Lname }).ToListAsync();





            if (!boSites.Any())
            {
                return Json(new { success = false, message = "No BO sites found for the provided OID." });
            }
            // Set EPPlus license context for .NET Core
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; 
            // Create a new Excel package
            using (var package = new ExcelPackage())
            {
                // Add a worksheet
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                // Add headers to the worksheet

                worksheet.Cells[1, 1].Value = "LocationName";//"LocationCode"; // lcode
                //worksheet.Cells[1, 1].Value = "LocationName";  // lname
                worksheet.Cells[1, 2].Value = "OvalId";
                worksheet.Cells[1, 3].Value = "ClientName";
                worksheet.Cells[1, 4].Value = "GeneralContractor(GC)";
                worksheet.Cells[1, 5].Value = "ProjectAddress";
                worksheet.Cells[1, 6].Value = "NatureofWork";
                worksheet.Cells[1, 7].Value = "ProjectArea";
                worksheet.Cells[1, 8].Value = "ProjectCost(est)";
                worksheet.Cells[1, 9].Value = "ProjectStartDate(est)";
                worksheet.Cells[1, 10].Value = "ProjectEndDate(est)";
                worksheet.Cells[1, 11].Value = "VendorCount";
                worksheet.Cells[1, 12].Value = "WorkerHeadcount";
                worksheet.Cells[1, 13].Value = "ProjectLead";
                

               
                // Populate rows with BOCW site details
                int row = 2;
                foreach (var site in boSites)
                {
                    //worksheet.Cells[row, 1].Value = site.Lcode;
                    worksheet.Cells[row, 1].Value = site.Lname;//site.Lcode;//site.Lname;
                   
                    row++;            
                }

                // Auto-fit columns for better readability
                worksheet.Cells.AutoFitColumns();

                // Generate file content
                var fileContent = package.GetAsByteArray();
                // Return file as download
                return File(fileContent,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Location Template.xlsx");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ViewBoDetails(string oid)
        {
            if (string.IsNullOrEmpty(oid))
            {
                return RedirectToAction("Error", new { message = "OID is required." });
            }

            var boDetails = await _organisationsetupservice.GetAllBocwDetailsAsync(oid);
            return PartialView(boDetails);
        }

        

        [HttpGet]
        public async Task<IActionResult> EditBoDetail(string lcode)
        {
            if (string.IsNullOrEmpty(lcode))
            {
                return RedirectToAction("Error", new { message = "Lcode is required." });
            }

            var boDetail = await _organisationsetupservice.GetBocwDetailsByLcodeAsync(lcode);
            if (boDetail == null)
            {
                return RedirectToAction("Error", new { message = "BO detail not found." });
            }
            return View(boDetail);
        }

        [HttpPost]
        public async Task<IActionResult> EditBoDetail(Ncmlocbo updatedBoDetail)                                     // Updating NCMLOCBO if any changes    
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please correct the errors in the form.";
                _logger.LogWarning("Form submission is invalid. Errors: {@ModelState}", ModelState);
                return View(updatedBoDetail);
            }

            try
            {
               

                await _organisationsetupservice.UpdateBoDetailsAsync(updatedBoDetail);
                TempData["SuccessMessage"] = "BO details updated successfully!";
                _logger.LogInformation("Successfully updated BO details for Lcode: {Lcode}", updatedBoDetail.Lcode);
                return RedirectToAction("ViewBoDetails", new { oid = updatedBoDetail.Lcode.Substring(0, 6) });
            }

            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                _logger.LogError(ex, "Failed to update BO details for Lcode: {Lcode}. Record not found.", updatedBoDetail.Lcode);
                return RedirectToAction("Error", new { message = ex.Message });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while updating BO details.";
                _logger.LogError(ex, "An unexpected error occurred while updating BO details for Lcode: {Lcode}", updatedBoDetail.Lcode);
                return View(updatedBoDetail);
            }
          
        }


        public async Task<IActionResult> Index()
       
        {
            try
            {
                var sites = await _organisationsetupservice.GetAllSitesAsync();
                return View(sites);
            }
            catch (Exception ex)
            {
                // Log error and return an error view/message
                // Logger.LogError(ex, "Error fetching sites");
                return View("Error");
            }

        }


        [HttpGet]
        public async Task<IActionResult> GetScopesBySite(string lcode, string projectCode)
        {
            try
            {
                // Fetch scopes for the site from BocwScope
                var scopes = await _organisationsetupservice.GetScopesAsync();
                return Json(scopes);
            }
            catch (Exception ex)
            {
                // Log error
                // Logger.LogError(ex, "Error fetching scopes");
                return Json(new List<BocwScope>());
            }
        }

       
        [HttpPost]
        public async Task<IActionResult> SaveMapping (string lcode, string projectCode, List<string> selectedScopeIds)
        {
            if (selectedScopeIds == null || !selectedScopeIds.Any())
            {
                ModelState.AddModelError("", "Please select at least one scope.");
                return RedirectToAction("Index");
            }

            await _organisationsetupservice.AddOrUpdateMapping(lcode, projectCode, selectedScopeIds);
            return RedirectToAction ("Index");
        }
            

        

    }
}


    





