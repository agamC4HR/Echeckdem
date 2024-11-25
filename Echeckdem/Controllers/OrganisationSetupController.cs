﻿using Microsoft.AspNetCore.Mvc;
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
        //[HttpGet]                                                                                                        // Get Organisation List View
        //public async Task<IActionResult> List(string searchTerm = "")
        //{
        //    var organisationList = await _organisationsetupservice.GetActiveOrganisationsListAsync(searchTerm);
        //    ViewData["CurrentFilter"] = searchTerm;
        //    return View("OrganisationList", organisationList);
        //}

        //[HttpGet]
        //public async Task<IActionResult> Details(string id)
        //{
        //    if (string.IsNullOrEmpty(id))
        //    {
        //        return BadRequest("Invalid organisation ID.");
        //    }

        //    var organisationInfo = await _organisationsetupservice.GetOrganisationGeneralInformationAsync(id);           // Get Organisation GerneralInfo Details
        //    if (organisationInfo == null)
        //    {
        //        return NotFound();
        //    }
        //    //return View("OrganisationList", organisationList);
        //    return View("Details", organisationInfo);
        //}


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




    }
}








