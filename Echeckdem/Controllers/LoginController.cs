using Echeckdem.Services;
using Microsoft.AspNetCore.Mvc;
using Echeckdem.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Echeckdem.CustomFolder;

namespace Echeckdem.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _loginService;
        private readonly IJwtService _jwtService;
        public LoginController(IUserService loginService, IJwtService jwtService)
        {
            _loginService = loginService;
            _jwtService = jwtService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        { 
            if (ModelState.IsValid)
            {
                var isValidUser = _loginService.IsValidUser(model.userID, model.password);

                if (isValidUser)
                {
                    // Retrieve user level and UNO
                    var userLevel = await _loginService.GetUserLevelAsync(model.userID);
                    var uno = await _loginService.GetUserUnoAsync(model.userID);                                   
                    var token = _jwtService.GenerateJwtToken(model);

                    HttpContext.Session.SetString("JWTToken", token);
                    HttpContext.Session.SetInt32("User Level", userLevel);
                    HttpContext.Session.SetString("userID", model.userID);
                    HttpContext.Session.SetInt32("UNO", uno);

                    //var locationTypes = await _loginService.GetUserLocationTypesAsync(uno);

                    //bool allSitesAreSOrF = locationTypes.All(l => l == "S" || l == "F");

                    //if (allSitesAreSOrF)
                    //{
                    //    return RedirectToAction("Index", "Home"); // Shows Home.cshtml
                    //}
                    //else
                    //{
                    //    return RedirectToAction("OtherView", "OtherController"); // Redirect elsewhere
                    //}

                    if (userLevel == 2) { Console.WriteLine("USERLEVEL:", userLevel.ToString()); }

                    ViewBag.UserLevel = userLevel;

                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ViewBag.Message = "Incorrect login ID or password.";
                    return View(model);
                }
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            // Clear the user session
            HttpContext.Session.Clear();
            //HttpContext.Session.Remove("UserID");

            // Redirect to the login page
            return RedirectToAction("Index", "Login");
        }
    }
}


