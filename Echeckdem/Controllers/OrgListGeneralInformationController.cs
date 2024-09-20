using Echeckdem.Services;
using Microsoft.AspNetCore.Mvc;

namespace Echeckdem.Controllers
{
    public class OrgListGeneralInformationController : Controller
    {
        private readonly OrgListGeneralInformationService _orglistGenInfoService;

        public OrgListGeneralInformationController(OrgListGeneralInformationService orgListGenInfoService)
        {
            _orglistGenInfoService = orgListGenInfoService;
        }

        [HttpGet]
        public async Task<IActionResult> GeneralInfo(string id) // id as string
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Invalid organisation ID.");
            }

            var info = await _orglistGenInfoService.GetOrganisationGeneralInformation(id);

            if (info == null)
            {
                return NotFound();
            }

            return View("~/Views/OrgList/GeneralInfo.cshtml", info);
        }
       /* public IActionResult Index()
        {
            return View();
        }*/
    }
}
