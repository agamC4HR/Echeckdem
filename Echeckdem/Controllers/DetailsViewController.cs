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

        public async Task<IActionResult> CombinedDetailed(int ulev, string uno, string organizationName = null, string site = null, string state = null, string city = null, string category = null)
        {
            var registrations = await _regService.GetDataAsync(ulev, uno, organizationName, site, state, city);
            var contributions = await _contService.GetDataAsync(ulev, uno, organizationName, site, state, city);
            var returns = await _retService.GetDataAsync( ulev, uno, organizationName, site, state, city);

            var detailedViewModel = new CombinedDetailedViewModel
            {
                Registrations = registrations,
                Contributions = contributions,
                Returns = returns,
                OrganizationName = organizationName,
                Site = site,
                State = state,
                City = city,
                Category = category
            };


            return View("~/Views/DetailedView/CombinedDetailedView.cshtml", detailedViewModel);
        }



    }
}

//        public IActionResult Index()
//        {
//            return View("~/Views/DetailedView/Filter.cshtml");
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetRegisterations()
//        {

//            // First function
//            var userLevelStr = HttpContext.Session.GetInt32("UserLevel");
//            var userID = HttpContext.Session.GetString("userID");

//            if (userLevelStr.HasValue)
//            {
//                // Use userLevel.Value as an integer
//                var registrations = await _regService.GetDataAsync(userLevelStr.Value, userID);
//                return View("~/Views/DetailedView/Registration.cshtml", registrations);
//            }

//            return RedirectToAction("Error", "Home"); // Handle invalid user level
//        }

//        [HttpGet]

//        public async Task<IActionResult> GetContributions()
//        {
//            var userLevelStr = HttpContext.Session.GetString("UserLevel");
//            var userID = HttpContext.Session.GetString("UserID");

//            if (int.TryParse(userLevelStr, out int userLevel))
//            {
//                var contributions = await _contService.GetDataAsync(userLevel, userID);
//                return View("~/Views/DetailedView/Contribution.cshtml", contributions);
//            }

//            return RedirectToAction("Error", "Home"); // Handle invalid user level
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetReturns()
//        {
//            var userLevelStr = HttpContext.Session.GetString("UserLevel");
//            var userID = HttpContext.Session.GetString("UserID");

//            if (int.TryParse(userLevelStr, out int userLevel))
//            {
//                var returns = await _retService.GetDataAsync(userLevel, userID);
//                return View("~/Views/DetailedView/Returns.cshtml", returns);
//            }

//            return RedirectToAction("Error", "Home"); // Handle invalid user level
//        }

//        [HttpPost]

//        public async Task<IActionResult> FilterData(FilterDetailedViewModel filter)
//        {
//            var userLevelStr = HttpContext.Session.GetString("UserLevel");
//            var userID = HttpContext.Session.GetString("userID");

//            if (!int.TryParse(userLevelStr, out int userLevel))
//            {
//                return RedirectToAction("Error", "Home");   // Handle unvalud userlevel
//            }

//            if (string.IsNullOrEmpty(userID))
//            {
//                return RedirectToAction("Error", "Home"); // Handle invalid user ID
//            }
//            {

//                List<object> filteredData = new List<object>();

//                if (filter.Returns)
//                {
//                    var returnsData = await _retService.GetDataAsync(userLevel, userID, filter.OName, filter.Lname);
//                    filteredData.AddRange(returnsData);
//                }

//                if (filter.Registrations)
//                {
//                    var registrationsData = await _regService.GetDataAsync(userLevel, userID, filter.OName, filter.Lname);
//                    filteredData.AddRange(registrationsData);
//                }

//                if (filter.Contributions)
//                {
//                    var contributionsData = await _contService.GetDataAsync(userLevel, userID, filter.OName, filter.Lname);
//                    filteredData.AddRange(contributionsData);
//                }

//                return View("~/Views/DetailedView/FilterData.cshtml", filteredData);
//            }



//        }
//    }
//}


