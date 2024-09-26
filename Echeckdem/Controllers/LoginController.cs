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
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        public LoginController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
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
                var user = await _userService.AuthenticateUserAsync(model.userID, model.password);

                if (user != null)
                {
                    // Retrieve user level
                    var userLevel = await _userService.GetUserLevelAsync(user.userID);

                    // Generate JWT token
                        var token = _jwtService.GenerateJwtToken(user);
                    

                    // Storing UserLevel in session

                    HttpContext.Session.SetInt32("UserLevel", userLevel);
                    HttpContext.Session.SetString("userID", user.userID);
                    // Redirect based on user level
                    switch (userLevel)
                    {
                        case 1:
                            return RedirectToAction("Index", "Admin"); // Admin page
                        //case 2:
                        case 2:
                            return RedirectToAction("Reports", "Reports"); // Reports page
                        //case 4:
                        case 3:
                            return RedirectToAction("Index", "DetailsView"); // Details View page
                        case 4:
                            return RedirectToAction("MainData", "MainData"); // Main Data page                                      
                        default:    
                            return RedirectToAction("Error", "Home"); // Handle unknown user levels
                    }
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
