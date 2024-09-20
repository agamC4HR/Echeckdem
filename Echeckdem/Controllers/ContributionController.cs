using Microsoft.AspNetCore.Mvc;
using Echeckdem.Services;

namespace Echeckdem.Controllers
{
    public class ContributionController : Controller
    {
        private readonly ContributionService _contService;

        public ContributionController(ContributionService contService)
        {
            _contService = contService;
        }
     
        public async Task<IActionResult> Index(int ulev, string uno)
        {
            var ContributionData = await _contService.GetDataAsync(ulev, uno);

            return View("~/Views/DetailedView/Contribution.cshtml", ContributionData);

        }
    }
} 