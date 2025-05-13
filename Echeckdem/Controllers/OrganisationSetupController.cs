using OfficeOpenXml;
using Microsoft.AspNetCore.Mvc;
using Echeckdem.Services;
using Echeckdem.CustomFolder;
using System.Security.Cryptography;
using Echeckdem.Models;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using DocumentFormat.OpenXml.Office.CustomXsn;
using DocumentFormat.OpenXml.InkML;
using OfficeOpenXml.DataValidation;

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

        //[HttpGet]
        //public IActionResult AddOrganisation()                                                                                 // Add Organisation details (setting up new organisation)
        //{

        //    return PartialView("AddOrganisation");
        //}

        [HttpGet]
        public async Task<IActionResult> AddOrganisation()
        {
            var model = new OrganisationGeneralInfoViewModel
            {
                SpocList = await _organisationsetupservice.GetC4HRSPOCListAsync()
            };
            return PartialView("AddOrganisation", model);
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
        public async Task<IActionResult> OrganisationSetup(string? searchTerm, string? selectedOid, bool? isActiveFilter) // added isActiveFilter paramter here acc to feedback.
        {
            var viewModel = await _organisationsetupservice.GetOrganisationSetupAsync(searchTerm, selectedOid, isActiveFilter);

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

        [HttpGet]                                                                                                                                // Add Locations process        --  1)  BULK  UPLOAD 
        public IActionResult Upload(string oid)
        {
            ViewBag.SelectedOid = oid;
            return PartialView("bulkupload");
        }

        [HttpPost]                                                                                                                                   // Add Locations process        --  1)  BULK UPLOAD
        public async Task<IActionResult> Upload(IFormFile file, string oid)
        {
            try
            {

                if (String.IsNullOrEmpty(oid))
                {
                    return Json(new { success = false, message = "*Error: Please select an organisation." });
                }
                if (file == null || file.Length == 0)
                {
                    return Json(new { success = false, message = "*Error: Please upload a valid Excel file." });
                }

                var recordCount = await _bulkUploadService.UploadLocationDataAsync(file, oid);
                return Json(new { success = true, message = $"{recordCount} records uploaded successfully." });
            }

            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public IActionResult DownloadExcelFile()                             // Downloading excel tempelate file 
        {
            // Set EPPlus license context for .NET Core
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var stateNames = _EcheckContext.Maststates.Select(s => s.Statedesc).ToList();



            // Predefined values for "Site Act"
            var siteActValues = new List<string> { "S", "F", "BO" };

            var boolValues = new List<string> { "Yes", "No" };

            // Create a new Excel package
            using (var package = new ExcelPackage()) //
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
                worksheet.Cells[1, 10].Value = "Site Escalation1 Email";
                worksheet.Cells[1, 11].Value = "Under CentralGovt";
                worksheet.Cells[1, 12].Value = "Under CLRA";
                worksheet.Cells[1, 13].Value = "Setup Year";
                worksheet.Cells[1, 14].Value = "Site Active";
                worksheet.Cells[1, 15].Value = "Under PF";
                worksheet.Cells[1, 16].Value = "Under ESI";

                var validationSheet = package.Workbook.Worksheets.Add("Validation"); // Add a hidden sheet for validation
                // Populate state names in the validation sheet
                //for (int i = 0; i < stateNames.Count; i++)
                //{
                //    validationSheet.Cells[i + 1, 1].Value = stateNames[i]; // Populate state names in column A
                //}
                for (int i = 0; i < stateNames.Count; i++)
                {
                    validationSheet.Cells[i + 1, 1].Value = stateNames[i];
                }

                // Populate site act values in the validation sheet
                for (int i = 0; i < siteActValues.Count; i++)
                {
                    validationSheet.Cells[i + 1, 2].Value = siteActValues[i];
                }

                // Populate "Yes" and "No" values in the validation sheet (Column C)
                for (int i = 0; i < boolValues.Count; i++)
                {
                    validationSheet.Cells[i + 1, 3].Value = boolValues[i];
                }




                // range for state dropdown
                var stateRange = validationSheet.Cells[1, 1, stateNames.Count, 1]; // Range of state names
                var validation = worksheet.DataValidations.AddListValidation("C2:C1000"); // Apply to "Site State" column (up to 1000 rows)
                validation.ShowErrorMessage = true;
                validation.ErrorTitle = "Invalid State";
                validation.Error = "Please select a state from the dropdown.";
                validation.Formula.ExcelFormula = $"Validation!$A$1:$A${stateNames.Count}";


                // range for Site Act dropdown
                var siteActRange = validationSheet.Cells[1, 2, siteActValues.Count, 2];
                var siteActValidation = worksheet.DataValidations.AddListValidation("E2:E1000"); // Apply to "Site Act" column
                siteActValidation.ShowErrorMessage = true;
                siteActValidation.ErrorTitle = "Invalid Site Act";
                siteActValidation.Error = "Please select a valid Site Act from the dropdown.";
                siteActValidation.Formula.ExcelFormula = $"Validation!$B$1:$B${siteActValues.Count}";  // Reference the site act values in the validation sheet



                // Email validation for "Site FM Email" (Column H)
                var fmEmailValidation = worksheet.DataValidations.AddCustomValidation("H2:H1000");
                fmEmailValidation.Formula.ExcelFormula = "AND(ISNUMBER(SEARCH(\"@\",H2)), ISNUMBER(SEARCH(\".\",H2)))";
                fmEmailValidation.ShowErrorMessage = true;
                fmEmailValidation.ErrorTitle = "Invalid Email";
                fmEmailValidation.Error = "Invalid Email Format";

                // Validation for "Site FM Contact Number" (Column I)
                var contactValidation = worksheet.DataValidations.AddCustomValidation("I2:I1000");
                contactValidation.Formula.ExcelFormula = "AND(ISNUMBER(I2),LEN(I2)=10)";
                contactValidation.ShowErrorMessage = true;
                contactValidation.ErrorTitle = "Invalid Contact Number";
                contactValidation.Error = "Please enter a 10-digit contact number.";


                // Email validation for "Site Escalation1" (Column J)
                var escalationEmailValidation = worksheet.DataValidations.AddCustomValidation("J2:J1000");
                escalationEmailValidation.Formula.ExcelFormula = "AND(ISNUMBER(SEARCH(\"@\",J2)), ISNUMBER(SEARCH(\".\",J2)))";
                escalationEmailValidation.ShowErrorMessage = true;
                escalationEmailValidation.ErrorTitle = "Invalid Email";
                escalationEmailValidation.Error = "Invalid Email Format";


                // range for dropdown values of yes and no for central govt, clra, site active.
                var boolRange = validationSheet.Cells[1, 3, boolValues.Count, 3];

                var centralGovtValidation = worksheet.DataValidations.AddListValidation("K2:K1000");
                centralGovtValidation.ShowErrorMessage = true;
                centralGovtValidation.ErrorTitle = "Invalid Entry";
                centralGovtValidation.Error = "Please select Yes or No.";
                centralGovtValidation.Formula.ExcelFormula = $"Validation!$C$1:$C${boolValues.Count}";

                var clraValidation = worksheet.DataValidations.AddListValidation("L2:L1000");
                clraValidation.ShowErrorMessage = true;
                clraValidation.ErrorTitle = "Invalid Entry";
                clraValidation.Error = "Please select Yes or No.";
                clraValidation.Formula.ExcelFormula = $"Validation!$C$1:$C${boolValues.Count}";

                var siteActiveValidation = worksheet.DataValidations.AddListValidation("N2:N1000");
                siteActiveValidation.ShowErrorMessage = true;
                siteActiveValidation.ErrorTitle = "Invalid Entry";
                siteActiveValidation.Error = "Please select Yes or No.";
                siteActiveValidation.Formula.ExcelFormula = $"Validation!$C$1:$C${boolValues.Count}";

                var pfValidation = worksheet.DataValidations.AddListValidation("O2:O1000");
                pfValidation.ShowErrorMessage = true;
                pfValidation.ErrorTitle = "Invalid Entry";
                pfValidation.Error = "Please select Yes or No.";
                pfValidation.Formula.ExcelFormula = $"Validation!$C$1:$C${boolValues.Count}";

                var esiValidation = worksheet.DataValidations.AddListValidation("P2:P1000");
                esiValidation.ShowErrorMessage = true;
                esiValidation.ErrorTitle = "Invalid Entry";
                esiValidation.Error = "Please select Yes or No.";
                esiValidation.Formula.ExcelFormula = $"Validation!$C$1:$C${boolValues.Count}";
                  
                // Hide the validation sheet
                validationSheet.Hidden = eWorkSheetHidden.Hidden;
                // Generate file content
                var fileContent = package.GetAsByteArray();
                // Return file as download
                return File(fileContent,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Location Setup Template.xlsx");
            }
        }

        [HttpGet]
        public IActionResult GetLocationDatabyOid(string oid)                                     // Viewing the Location DATA tab
        {
            if (string.IsNullOrEmpty(oid))
            {
                TempData["ErrorMessage"] = "Invalid OID.";
                return RedirectToAction("OrganisationSetup");
            }

            var locations = _organisationsetupservice.GetLocationDatabyOidAsync(oid).Result;

            // Create an instance of CombinedOrganisationSetupViewModel 
            var model = new CombinedOrganisationSetupViewModel
            {
                AddLocation = locations, 
                oid = oid 
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

                string[] headers = {
                "SiteName", "OvalId", "ClientName", "GeneralContractor(GC)", "ProjectAddress",
                "NatureofWork", "ProjectArea(in sq.ft)", "ProjectCost(est)", "ProjectStartDate(est)",
                "ProjectEndDate(est)", "VendorCount", "WorkerHeadcount", "ProjectLead"
                };

                for (int col = 0; col < headers.Length; col++)
                {
                    worksheet.Cells[1, col + 1].Value = headers[col];
                }

                // Force Excel to store the date in dd/MM/yyyy
                worksheet.Cells[2, 9, 100, 9].Style.Numberformat.Format = "dd/MM/yyyy"; // Start Date
                worksheet.Cells[2, 10, 100, 10].Style.Numberformat.Format = "dd/MM/yyyy"; // End Date

                // Force users to pick date using Excel’s Date Picker
                var startDateValidation = worksheet.DataValidations.AddDateTimeValidation("I2:I100"); // Column 9
                startDateValidation.ShowErrorMessage = true;
                startDateValidation.ErrorTitle = "Invalid Date Format";
                startDateValidation.Error = "Please use the date picker to enter a valid date in DD/MM/YYYY format.";
                startDateValidation.Operator = ExcelDataValidationOperator.between;
                startDateValidation.Formula.Value = new DateTime(2000, 1, 1);
                startDateValidation.Formula2.Value = new DateTime(2100, 12, 31);

                var endDateValidation = worksheet.DataValidations.AddDateTimeValidation("J2:J100"); // Column 10
                endDateValidation.ShowErrorMessage = true;
                endDateValidation.ErrorTitle = "Invalid Date Format";
                endDateValidation.Error = "Please use the date picker to enter a valid date in DD/MM/YYYY format.";
                endDateValidation.Operator = ExcelDataValidationOperator.between;
                endDateValidation.Formula.Value = new DateTime(2000, 1, 1);
                endDateValidation.Formula2.Value = new DateTime(2100, 12, 31);





                // Populate rows with BOCW site details
                int row = 2;
                foreach (var site in boSites)
                {
                    worksheet.Cells[row, 1].Value = site.Lname;
                    row++;
                }

                // Auto-fit columns for better readability
                worksheet.Cells.AutoFitColumns();

                // Generate file content
                var fileContent = package.GetAsByteArray();
                // Return file as download
                return File(fileContent,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "BOCW Site Setup Template.xlsx");
            }
        }

        //<<---------------------START----------------------------------VIEW DATA FOR BOCW SITES----------------------------------------->

        [HttpGet]
        public async Task<IActionResult> ViewBoDetails(string oid)                                             // View BOCW sites details under VIEW BOCW DATA Button
        {
            if (string.IsNullOrEmpty(oid))
            {
                return RedirectToAction("Error", new { message = "OID is required." });
            }

            var boDetails = await _organisationsetupservice.GetAllBocwDetailsWithScopesAsync(oid);
            return PartialView(boDetails);
        }

        //<<---------------------END----------------------------------VIEW DATA FOR BOCW SITES----------------------------------------->
        // <<--------------------START-------------------------------------------EDIT for BOCW SITE DATA----------------------------------------------------------------------->>
        [HttpGet]
        public async Task<IActionResult> GetEditNcmlocbo(string lcode)                 // editing the details in ncmlocbo
        {
            if (string.IsNullOrEmpty(lcode))
            {
                return Json(new { success = false, message = "Invalid Lcode." });
            }

            var boDetail = await _EcheckContext.Ncmlocbos.FirstOrDefaultAsync(b => b.Lcode == lcode);
            if (boDetail == null)
            {
                return Json(new { success = false, message = "BOCW site not found." });
            }

            return PartialView("_EditNcmlocbo", boDetail);
        }

        [HttpPost]


        public async Task<IActionResult> UpdateNcmlocbo(Ncmlocbo updatedBo, string lcode)                            // editing the details in ncmlocbo
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid data." });
            }

            var existingBo = await _EcheckContext.Ncmlocbos.FirstOrDefaultAsync(b => b.Lcode == lcode);
            if (existingBo == null)
            {
                return Json(new { success = false, message = "BO site not found." });
            }

            // Update the properties
            existingBo.ProjectCode = updatedBo.ProjectCode;
            existingBo.OvalId = updatedBo.OvalId;
            existingBo.ClientName = updatedBo.ClientName;
            existingBo.GeneralContractor = updatedBo.GeneralContractor;
            existingBo.ProjectAddress = updatedBo.ProjectAddress;
            existingBo.NatureofWork = updatedBo.NatureofWork;
            existingBo.ProjectArea = updatedBo.ProjectArea;
            existingBo.ProjectCostEst = updatedBo.ProjectCostEst;
            existingBo.ProjectStartDateEst = updatedBo.ProjectStartDateEst;
            existingBo.ProjectEndDateEst = updatedBo.ProjectEndDateEst;
            existingBo.VendorCount = updatedBo.VendorCount;
            existingBo.WorkerHeadCount = updatedBo.WorkerHeadCount;
            existingBo.ProjectLead = updatedBo.ProjectLead;
            existingBo.Lname = updatedBo.Lname;
            existingBo.ActiveScopes = updatedBo.ActiveScopes;


            await _EcheckContext.SaveChangesAsync();
            TempData["SuccessMessage"] = "BO site updated successfully.";

            return Json(new { success = true, message = "BO site updated successfully." });
        }
        // <<--------------------END-------------------------------------------EDIT for BOCW SITE DATA----------------------------------------------------------------------->>

        //-----------------------START-----------------------------------------SCOPE SETUP-------------------------------------------------------------------------------------------------//
        public async Task<IActionResult> Index()                                                                        // Get all sites under bocw that can be further used for scope mapping.
        {
            try
            {
                var sites = await _organisationsetupservice.GetAllSitesAsync();
                return View(sites);
            }
            catch (Exception ex)
            {
                return View("Error");
            }

        }

       
        [HttpGet]

        public async Task<IActionResult> GetScopesPartial(string lcode, string projectCode)                                 //  
        {
            try
            {
                // Get all available scopes
                var scopes = await _organisationsetupservice.GetScopesAsync();

                // Get existing mappings
                var existingMappings = await _EcheckContext.BoScopeMaps
                    .Where(m => m.Lcode == lcode && m.ProjectCode == projectCode)
                    .Select(m => m.ScopeId)
                    .ToListAsync();

                foreach (var scope in scopes)
                {
                    scope.IsSelected = existingMappings.Contains(scope.ScopeId);
                }

                ViewBag.Lcode = lcode;
                ViewBag.ProjectCode = projectCode;
                return PartialView("_ScopesMapping", scopes);
            }
            catch (Exception ex)
            {
                // Log error
                Console.WriteLine($"Error in GetScopesPartial: {ex.Message}");
                return PartialView("_ScopesMapping", new List<BocwScope>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveMapping(string lcode, string projectCode, List<string> selectedScopeIds)
        {

            try
            {


                if (selectedScopeIds == null || !selectedScopeIds.Any())
                {
                    ModelState.AddModelError("", "Please select at least one scope.");
                    return Json(new { success = false, message = "No scopes selected." });

                }

                await _organisationsetupservice.AddOrUpdateMapping(lcode, projectCode, selectedScopeIds);

                TempData["SuccessMessage"] = "Scope setup was successful!";


                var oid = await _organisationsetupservice.GetOidByLcodeAsync(lcode);
                if (string.IsNullOrEmpty(oid))
                {
                    return Json(new { success = false, message = "OID not found for the specified Lcode." });
                }

                return Json(new { success = true, oid, refresh = true });

            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving scope mapping for Lcode: {Lcode}, ProjectCode: {ProjectCode}", lcode, projectCode);
                TempData["ErrorMessage"] = "An error occurred while saving the mapping. Please try again.";
                return Json(new { success = false, message = "An error occurred while saving the mapping. Please try again." });
            }
        }

        //-------------------------END------------------------SCOPE SETUP-------------------------------------------------------------------------------------------------//

        //-------------------------START------------------------PROJECT SETUP AFTER SCOPE SETUP-------------------------------------------------------------------------------//

        [HttpGet]

        public async Task<IActionResult> PopulateNCBOCW(string lcode, string projectCode)
        {
            try
            {
                
                var transactionId = await _organisationsetupservice.ProjectSetupAsync(lcode, projectCode,HttpContext);
                     return Json(new { success = true, transactionId });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        //-------------------------END-------------------PROJECT SETUP AFTER SCOPE SETUP-------------------------------------------------------------------------------//



    }
}


    





