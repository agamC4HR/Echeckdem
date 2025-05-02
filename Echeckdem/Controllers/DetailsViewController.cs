using Echeckdem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using Echeckdem.Services;
using System.Runtime.CompilerServices;
using Echeckdem.CustomFolder;

namespace Echeckdem.Controllers
{
    public class DetailsViewController : Controller
    {   
        private readonly RegistrationService _regService;
        private readonly ContributionService _contService;
        private readonly ReturnsService _retService;


        public DetailsViewController(RegistrationService regService, ContributionService contService, ReturnsService retService)
        {
            _regService = regService;
            _contService = contService;
            _retService = retService;
            
        }

        public async Task<IActionResult> CombinedDetailed(string organizationName = null,  string LocationName = null!, string StateName = null, string CityName = null, DateOnly? StartDueDate = null, DateOnly? EndDueDate = null, DateOnly? StartPeriod = null, DateOnly? EndPeriod = null)//(int ulev, int uno, string organizationName = null)
        {

            int ulev = HttpContext.Session.GetInt32("User Level") ?? 0;
            int uno = HttpContext.Session.GetInt32("UNO") ?? 0;


            if (ulev == 0)// || uno == 0)
            {
                // If session values are missing, redirect to login or show error 
                TempData["ErrorMessage"] = "Session has expired. Please log in again.";
                return RedirectToAction("Index", "Login");
            }



            var registrations = await _regService.GetDataAsync(ulev, uno, organizationName, LocationName, StateName, CityName, StartDueDate, EndDueDate, StartPeriod, EndPeriod);
            var contributions = await _contService.GetDataAsync(ulev, uno, organizationName, LocationName, StateName, CityName, StartDueDate, EndDueDate, StartPeriod, EndPeriod);
            var returns = await _retService.GetDataAsync(ulev, uno, organizationName, LocationName, StateName, CityName, StartDueDate, EndDueDate, StartPeriod, EndPeriod);

            var detailedViewModel = new CombinedDetailedViewModel
            {
                Registrations = registrations,
                Contributions = contributions,
                Returns = returns,
                OrganizationName = organizationName,
                SiteName = LocationName,
                StateName = StateName,
                CityName = CityName,
                StartDueDate = StartDueDate,
                EndDueDate = EndDueDate,
                StartPeriod = StartPeriod,
                EndPeriod = EndPeriod
            };

            var organizationNames = await _regService.GetOrganizationNamesAsync(uno);
            ViewBag.OrganizationNames = organizationNames;

            var locationNames = string.IsNullOrEmpty(organizationName)
               ? await _regService.GetLocationNamesAsync(uno)
               : await _regService.GetFilteredLocationNamesAsync(uno, organizationName);
            ViewBag.LocationNames = locationNames;

            var StateNames = await _regService.GetStateNamesAsync(uno);
            ViewBag.StateNames = StateNames;

            var CityNames = await _regService.GetCityNamesAsync(uno);
            ViewBag.CityNames = CityNames;

            //var ReturnData = await _regService.GetDataAsync(ulev, uno, organizationName, LocationName, StateName, CityName);


            return View("~/Views/DetailedView/CombinedDetailedView.cshtml", detailedViewModel);     
        }
        
        [HttpGet]
        public async Task<IActionResult> GetLocations(string organizationName)  
        {
            int uno = HttpContext.Session.GetInt32("UNO") ?? 0;
                
            if (string.IsNullOrEmpty(organizationName))
            {
                return Json(await _regService.GetLocationNamesAsync(uno));
            }

            var locations = await _regService.GetFilteredLocationNamesAsync(uno, organizationName);
            return Json(locations);
        }

        [HttpGet]
        public async Task<IActionResult> GetReturnLocations(string organizationName)
        {
            int uno = HttpContext.Session.GetInt32("UNO") ?? 0;
            if (string.IsNullOrEmpty(organizationName))
                return Json(await _retService.GetLocationNamesAsync(uno));

            return Json(await _retService.GetFilteredLocationNamesAsync(uno, organizationName));
        }

        [HttpGet]
        public async Task<IActionResult> GetContributionLocations(string organizationName)
        {
            int uno = HttpContext.Session.GetInt32("UNO") ?? 0;
            if (string.IsNullOrEmpty(organizationName))
                return Json(await _contService.GetLocationNamesAsync(uno));

            return Json(await _contService.GetFilteredLocationNamesAsync(uno, organizationName));
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
        //----------------END----------------------------------REGISTRATION------------------------------------------------------------------------------//

        //----------------START----------------------------------CONTRIBUTION ------------------------------------------------------------------------------//

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
    }
}