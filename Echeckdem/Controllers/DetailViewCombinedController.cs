using Microsoft.AspNetCore.Mvc;
using Echeckdem.Services;

namespace Echeckdem.Controllers
{
    //[Route("api/unified-data")]
    //[ApiController]
    [Route("DetailViewCombined")]
    public class DetailViewCombinedController : Controller
    {
        private readonly DetailViewCombinedService _detailViewCombinedService;

        public DetailViewCombinedController(DetailViewCombinedService detailViewCombinedService)

        {
            _detailViewCombinedService = detailViewCombinedService;

        }

       

       
        public async Task<IActionResult> Index(int ulev, int uno)
        {
            var result = await _detailViewCombinedService.GetDataAsync(ulev, uno);
            return View(result);
        }

        [HttpGet("fetch")]
        public async Task<IActionResult> GetDataAsync(
       int ulev, int uno,
       string organizationName = null, string locationName = null,
       string stateName = null, string cityName = null,
       DateOnly? startDueDate = null, DateOnly? endDueDate = null,
       DateOnly? startPeriod = null, DateOnly? endPeriod = null)
        {
            var result = await _detailViewCombinedService.GetDataAsync(ulev, uno, organizationName, locationName, stateName, cityName, startDueDate, endDueDate, startPeriod, endPeriod);
            return Ok(result);
        }

        //[HttpGet("organizations")]
        //public async Task<IActionResult> GetOrganizations(int uno)
        //{
        //    return Ok(await _detailViewCombinedService.GetOrganizationNamesAsync(uno));
        //}
    }


}

