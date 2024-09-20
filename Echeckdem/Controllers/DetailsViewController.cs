using Echeckdem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using Echeckdem.Services;
using System.Runtime.CompilerServices;

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
        public IActionResult Index()
        {
            return View("~/Views/UserPages/DetailsView.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> GetRegisterations() 
        {
            var userLevelStr = HttpContext.Session.GetString("UserLevel");
            var userID = HttpContext.Session.GetString("UserID");
            Console.WriteLine(userLevelStr);
            if (int.TryParse(userLevelStr, out int userLevel))
            {
                var registrations = await _regService.GetDataAsync(userLevel, userID);
                return View("~/Views/DetailedView/Registration.cshtml", registrations);
            }

            return RedirectToAction("Error", "Home"); // Handle invalid user level
        }   

        [HttpGet]

        public async Task<IActionResult> GetContributions()
        {
            var userLevelStr = HttpContext.Session.GetString("UserLevel");
            var userID = HttpContext.Session.GetString("UserID");

            if (int.TryParse(userLevelStr, out int userLevel))
            {
                var contributions = await _contService.GetDataAsync(userLevel, userID);
                return View("~/Views/DetailedView/Contribution.cshtml", contributions);
            }

            return RedirectToAction("Error", "Home"); // Handle invalid user level
        }

        [HttpGet]
        public async Task<IActionResult> GetReturns()
        {
            var userLevelStr = HttpContext.Session.GetString("UserLevel");
            var userID = HttpContext.Session.GetString("UserID");

            if (int.TryParse(userLevelStr, out int userLevel))
            {
                var returns = await _retService.GetDataAsync(userLevel, userID);
                return View("~/Views/DetailedView/Returns.cshtml", returns);
            }

            return RedirectToAction("Error", "Home"); // Handle invalid user level
        }
       
    }
}


