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
                    
                    Console.WriteLine("UNO:", uno);
                    Console.WriteLine("USERID:", model.userID);
                    // Generate JWT token
                    //var token = await _jwtService.GenerateJwtToken(model.userID);

                    // Storing UserLevel in session


                    //ViewBag.UserLevel = userLevel;

                    HttpContext.Session.SetInt32("User Level", userLevel);
                    HttpContext.Session.SetString("userID", model.userID);
                    HttpContext.Session.SetInt32("UNO", uno);

                    if (userLevel==2) { Console.WriteLine("USERLEVEL:", userLevel.ToString()); }

                    ViewBag.UserLevel = userLevel;

                    return RedirectToAction("Index", "Home");

                    // Redirect based on user level
                    //switch (userLevel)
                    //{
                    //    case 1:
                    //        return RedirectToAction("Index", "Admin"); // Admin page
                    //    //case 2:
                    //    case 3:
                    //        return RedirectToAction("Reports", "Reports"); //9 Reports page
                    //    //case 4:
                    //    case 2:
                    //        return RedirectToAction("Index", "Home"); // Details View page
                    //    case 4:
                    //        return RedirectToAction("MainData", "MainData"); // Main Data page                                      
                    //    default:
                    //        return RedirectToAction("Error", "Home"); // Handle unknown user levels
                    //}
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

            // Redirect to the login page
            return RedirectToAction("Index", "Login");
        }
    }
}


