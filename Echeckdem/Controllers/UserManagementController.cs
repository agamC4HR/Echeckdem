using Echeckdem.Models;
using Echeckdem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Echeckdem.Controllers
{
    public class UserManagementController : Controller
    {

        private readonly IUserManagementService _userManagementService;
        private readonly DbEcheckContext _EcheckContext;


        public UserManagementController(IUserManagementService userManagementService, DbEcheckContext echeckContext)

        {
            _userManagementService = userManagementService;
            _EcheckContext = echeckContext;
        }
        public IActionResult Index()
        {
            var users = _userManagementService.GetUsers();
            return View(users);
        }

        public IActionResult Create()
        {
            ViewBag.Organizations = _userManagementService.GetOrganizations();

            ViewBag.UserLevels = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Admin" },
            new SelectListItem { Value = "2", Text = "SPOC" },
            new SelectListItem { Value = "3", Text = "Reports" },
            new SelectListItem { Value = "4", Text = "Data Entry User" },
            new SelectListItem { Value = "5", Text = "Data Viewer" },
            new SelectListItem { Value = "6", Text = "Planner" },
            new SelectListItem { Value = "7", Text = "Tax Team" },
            new SelectListItem { Value = "8", Text = "Payroll Team" },
            new SelectListItem { Value = "9", Text = "Company Secretary" }
            };

            return View();
        }

        [HttpPost]

        public IActionResult Create(Ncuser user)
        {
            if (ModelState.IsValid)
            {
                bool Isadded = _userManagementService.AddUser(user);
                if (Isadded)

                    return RedirectToAction("Index");
                else
                    ModelState.AddModelError("", "UserID already exists.");

            }

            ViewBag.Organizations = _userManagementService.GetOrganizations();
            return View(user);
        }

        public IActionResult MapUser(string userId)
        {
            //var user = _userManagementService.GetUserById(userId);  
           var user = _EcheckContext.Ncusers.FirstOrDefault(u => u.Userid == userId);
            if (user == null) return NotFound();

            ViewBag.UserId = user.Userid;
            ViewBag.Organizations = _userManagementService.GetOrganizations();
            ViewBag.UserLevels = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Reports" },
                new SelectListItem { Value = "5", Text = "Uploader" },
                new SelectListItem { Value = "10", Text = "Auditor" },
                new SelectListItem { Value = "15", Text = "Owner" }
            };

            return View();
        }

        [HttpPost]
        public IActionResult MapUser(string userId, Ncumap mapping)
        {
            if (ModelState.IsValid)
            {
                var user = _userManagementService.GetUserById(userId);
                if (user != null)
                {
                    mapping.Uno = user.Uno;
                    bool isMapped = _userManagementService.MapUserToOrgLocation(mapping);
                    if (isMapped)
                        return RedirectToAction("Index", "UserManagement");

                    ModelState.AddModelError("", "Mapping already exists!");
                }
                else
                {
                    ModelState.AddModelError("", "User not found!");
                }
             
            }

            ViewBag.Organizations = _userManagementService.GetOrganizations();
            ViewBag.UserLevels = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Reports" },
                new SelectListItem { Value = "5", Text = "Uploader" },
                new SelectListItem { Value = "10", Text = "Auditor" },
                new SelectListItem { Value = "15", Text = "Owner" }
            };

            return View(mapping);
        }

        public JsonResult GetLocations(string oid)
        {
            var locations = _userManagementService.GetLocationsByOrg(oid);
            return Json(locations);
        }
    }

}
