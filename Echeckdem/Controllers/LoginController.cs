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
                    // Retrieve user level
                    var userLevel = await _loginService.GetUserLevelAsync(model.userID);

                    // Generate JWT token
                    //var token = await _jwtService.GenerateJwtToken(model.userID);

                    // Storing UserLevel in session


                    //ViewBag.UserLevel = userLevel;

                    HttpContext.Session.SetInt32("User Level", userLevel);
                    HttpContext.Session.SetString("userID", model.userID);

                    var storedUserLevel = HttpContext.Session.GetInt32("UserLevel");
                    //Console.WriteLine($"Attempting to store UserLevel: {userLevel}");
                    //Console.WriteLine($"Stored UserLevel after setting: {storedUserLevel}");
                    Console.WriteLine("USERID:", model.userID);
                    // Redirect based on user level
                    return RedirectToAction("Index", "Home");
                    //switch (userLevel)
                    //{
                    //    case 1:
                    //        //return RedirectToAction("Index", "Admin"); // Admin page
                    //        return RedirectToAction("Index", "Home"); // Details View page
                    //    //case 2:
                    //    case 3:
                    //        return RedirectToAction("Reports", "Reports"); // Reports page
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
    }
}


