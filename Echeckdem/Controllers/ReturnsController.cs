using Echeckdem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace Echeckdem.Controllers
{
    public class ReturnsController : Controller
    {
        private readonly ReturnsService _retservice;

        public ReturnsController(ReturnsService retservice)
        {
            _retservice = retservice;
        }

        public async Task<IActionResult> Index(int ulev, string uno)
        {
            var ReturnData = await _retservice.GetDataAsync(ulev, uno);

            return View("~/Views/DetailedView/Returns.cshtml", ReturnData);
                    
        }
/*
        public IActionResult Index()
        {
            return View();
        }*/
    }
}
