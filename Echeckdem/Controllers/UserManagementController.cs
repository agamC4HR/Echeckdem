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
                    new SelectListItem { Value = "5", Text = "Data Viewer" }
          
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


            public IActionResult EditPartial(string userId)
            {
                if (string.IsNullOrEmpty(userId))
                    return NotFound();

                var user = _userManagementService.GetUserById(userId);
                if (user == null)
                    return NotFound();

                ViewBag.Organizations = _userManagementService.GetOrganizations();
                ViewBag.UserLevels = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Admin" },
            new SelectListItem { Value = "2", Text = "SPOC" },
            new SelectListItem { Value = "3", Text = "Reports" },
            new SelectListItem { Value = "4", Text = "Data Entry User" },
            new SelectListItem { Value = "5", Text = "Data Viewer" }
        
        };

                return PartialView("_EditPartial", user);
            }


            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Edit(Ncuser user)
            {
                if (ModelState.IsValid)
                {
                    bool isUpdated = _userManagementService.UpdateUser(user);
                    if (isUpdated)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Error updating user.");
                    }
                }

                ViewBag.Organizations = _userManagementService.GetOrganizations();
                ViewBag.UserLevels = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Admin" },
                new SelectListItem { Value = "2", Text = "SPOC" },
                new SelectListItem { Value = "3", Text = "Reports" },
                new SelectListItem { Value = "4", Text = "Data Entry User" },
                new SelectListItem { Value = "5", Text = "Data Viewer" }

            };

                   return View(user);
           

            }

            private Dictionary<int, string> _userLevelNames = new Dictionary<int, string>
                {
                    { 1, "Reports" },
                    { 5, "Uploader" },
                    { 10, "Auditor" },
                    { 15, "Owner" },
                    { 101, "Contribution" },
                    { 102, "Registration" },    
                    { 103, "Return" },
                    { 104, "Registration and Return" },
                    { 105, "All 3" }
                };
            public IActionResult MapUser(string userId)
            {
            
               var user = _EcheckContext.Ncusers.FirstOrDefault(u => u.Userid == userId);
                if (user == null) return NotFound();

          


            ViewBag.UserId = user.Userid;

                // Fetch the organizations authorized to the user
                var authorizedMappings = (from mapping in _EcheckContext.Ncumaps
                                          join org in _EcheckContext.Ncmorgs on mapping.Oid equals org.Oid
                                          join loc in _EcheckContext.Ncmlocs on mapping.Lcode equals loc.Lcode
                                          where mapping.Uno == user.Uno
                                          select new
                                          {
                                              mapping.Oid,
                                              org.Oname,
                                              loc.Lcode,
                                              loc.Lname,
                                              mapping.Ulevel
                                          })
                                   .GroupBy(m => new { m.Oid, m.Oname })
                                   .Select(g => new
                                   {
                                       g.Key.Oid,
                                       g.Key.Oname,
                                       Locations = g.Select(l => new
                                       {
                                           l.Lcode,
                                           l.Lname,
                                           l.Ulevel
                                       }).ToList()
                                   }).ToList();

                ViewBag.AuthorizedMappings = authorizedMappings;

                // Set user levels based on the user's current level
                if (user.Userlevel == 2 || user.Userlevel == 3)
                {
                    ViewBag.UserLevels = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Reports" },
                new SelectListItem { Value = "5", Text = "Uploader" },
                new SelectListItem { Value = "10", Text = "Auditor" },
                new SelectListItem { Value = "15", Text = "Owner" }
            };
                }
                else if (user.Userlevel == 4 || user.Userlevel == 5)
                {
                    ViewBag.UserLevels = new List<SelectListItem>
            {
                new SelectListItem { Value = "101", Text = "Contribution" },
                new SelectListItem { Value = "102", Text = "Registration" },
                new SelectListItem { Value = "103", Text = "Return" },
                new SelectListItem { Value = "104", Text = "Regs and Return" },
                new SelectListItem { Value = "105", Text = "All 3" }
            };
                }

                else
                {
                    {
                    
                        ViewBag.UserLevels = new List<SelectListItem>(); // No options for other levels  
                    }

                }

                ViewBag.UserLevelNames = _userLevelNames;
                ViewBag.Organizations = _userManagementService.GetOrganizations();
              

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

                        // Set the Ulevel based on the selected value
                        if (mapping.Ulevel == "1" || mapping.Ulevel == "5" || mapping.Ulevel == "10" || mapping.Ulevel == "15")
                        {
                            // Map to the corresponding values for Reports, Uploader, Auditor, Owner
                            mapping.Ulevel = mapping.Ulevel; // This will be 1, 5, 10, or 15
                        }
                        else if (mapping.Ulevel == "101" || mapping.Ulevel == "102" || mapping.Ulevel == "103" || mapping.Ulevel == "104" || mapping.Ulevel == "105")
                        {
                            // Map to the corresponding values for Contribution, Registration, Return, Regs and Return, All 3
                            mapping.Ulevel = mapping.Ulevel; // This will be 101, 102, 103, 104, or 105
                        }

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
      

                return View(mapping);
            }

            [HttpPost]
            public IActionResult UpdateUserLevel(int Uno, string userId, string lcode, string newLevel)
            {
                var mapping = _EcheckContext.Ncumaps.FirstOrDefault(m => m.Uno == Uno && m.Lcode == lcode);
                if (mapping == null)
                {
                    return NotFound();
                }

                mapping.Ulevel = newLevel;
                _EcheckContext.SaveChanges();
            
                return Json(new { success = true });
            }


            public JsonResult GetLocations(string oid)
            {
                var locations = _userManagementService.GetLocationsByOrg(oid);
                return Json(locations);
            }

      

        }

    }