using Echeckdem.Services;
using Microsoft.AspNetCore.Mvc;
using Echeckdem.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Echeckdem.CustomFolder;
using Echeckdem.Handlers;

namespace Echeckdem.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _loginService;
        private readonly IJwtService _jwtService;
        private readonly IAudtrail _audtrailService;
        public LoginController(IUserService loginService, IJwtService jwtService, IAudtrail audtrail)
        {
            _loginService = loginService;
            _jwtService = jwtService;
            _audtrailService = audtrail;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await _audtrailService.AddAuditTrailAsync("Anonymous","Login","Login page viewed.");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        { 
            if (ModelState.IsValid)
            {
                var isValidUser = _loginService.IsValidUser(model.userID, model.password);
                var isValidUserhash= _loginService.IsValidUserhash(model.userID, model.password);
                if (isValidUserhash)
                {
                    // Retrieve user level and UNO
                    var userLevel = await _loginService.GetUserLevelAsync(model.userID);
                    var uno = await _loginService.GetUserUnoAsync(model.userID);                                   
                    var token = _jwtService.GenerateJwtToken(model);

                    HttpContext.Session.SetString("JWTToken", token);
                    HttpContext.Session.SetInt32("User Level", userLevel);
                    HttpContext.Session.SetString("userID", model.userID);
                    HttpContext.Session.SetInt32("UNO", uno);

                    
                    if (userLevel == 2) { Console.WriteLine("USERLEVEL:", userLevel.ToString()); }

                    ViewBag.UserLevel = userLevel;

                    await _audtrailService.AddAuditTrailAsync(model.userID,"Login","User logged in successfully.");

                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ViewBag.Message = "Incorrect login ID or password.";
                    await _audtrailService.AddAuditTrailAsync(model.userID, "Login", "Failed login attempts with incorrect credentials.");
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


