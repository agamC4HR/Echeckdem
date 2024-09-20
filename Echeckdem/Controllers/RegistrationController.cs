using Echeckdem.Services;
using Microsoft.AspNetCore.Mvc;

namespace Echeckdem.Controllers
{
    public class RegistrationController : Controller

    {

        private readonly RegistrationService _regService;

        public RegistrationController(RegistrationService regService)
        {
            _regService = regService;
        }
        public async Task<IActionResult> Index(int ulev, string uno)
        {
            var ReturnData = await _regService.GetDataAsync(ulev, uno);

            return View("~/Views/DetailedView/Registration.cshtml", ReturnData);

        }

      
    }
}
