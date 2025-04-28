using DocumentFormat.OpenXml.Spreadsheet;
using Echeckdem.CustomFolder.UserManagement;
using Echeckdem.Models;
    using Echeckdem.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
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

        public async Task<IActionResult> Index()
        {
            var users = await _userManagementService.GetAllUsersAsync();
            return View(users);
        }

        public async Task<IActionResult> Create()
        {
            // Fetch all organisations for the dropdown
            var organisations = await _userManagementService.GetAllOrganisationsAsync();
            if (organisations == null || !organisations.Any())
            {
               
                ViewBag.Organisations = new List<OrganisationViewModel> { new OrganisationViewModel { OID = "0", OrganisationName = "No Organisations Available" } };
            }
            else
            {
                ViewBag.Organisations = new SelectList(organisations, "OID", "OrganisationName");
            }

            return View();
           
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _userManagementService.AddUserAsync(model);
                return RedirectToAction(nameof(Index)); // Redirection to user list after adding
            }

            
            var organisations = await _userManagementService.GetAllOrganisationsAsync();
            ViewBag.Organisations = new SelectList(organisations, "OID", "Oname");
            return View(model);
        }

       
        [HttpPost]
        public IActionResult MapOrganisation(string userId)
        {
           
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }
            //HttpContext.Session.SetString("UserId", userId);
            //var userId = HttpContext.Session.GetString("UserID");
            // userid storing in session so that wwe dontget data in link.

            HttpContext.Session.SetString("UserId", userId);

            return RedirectToAction("MapOrganisation");
        }


        [HttpGet]
        public async Task<IActionResult> MapOrganisation()

        {
            string userId = HttpContext.Session.GetString("UserId");
            //string userId = HttpContext.Session.GetString("UserId"); 

                                                                     
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("failed to get userid");
            }

            try
            {
                var mappings = await _userManagementService.GetUserMappingAsync(userId);
                ViewBag.UserId = userId;
                return View(mappings);
            }
            catch (Exception ex)
            {
                
                return NotFound(ex.Message);
            }
        }



        //public async Task<IActionResult> Index()
        //{
        //    var users = await _userManagementService.GetAllUsersAsync();
        //    var viewModelList = users.Select(user => new UserManagementViewModel
        //    {
        //        UserID = user.Userid,
        //        Uname = user.Uname,
        //        Password = user.Password,
        //        Oid = user.Oid,
        //        UNO = user.Uno,
        //        UserLevel = user.Userlevel.ToString(),
        //        Uactive = user.Uactive,
        //        EmailID = user.Emailid
        //    }).ToList();

        //    return View(viewModelList);
        //}

        //public async Task<IActionResult> Create()
        //{
        //    var viewModel = new UserManagementViewModel
        //    {
        //        UserLevelOptions = await _userManagementService.GetUserLevelOptionsAsync()
        //    };
        //    return PartialView("_Create", viewModel);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(UserManagementViewModel viewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = new Ncuser
        //        {
        //            Userid = Guid.NewGuid().ToString(),
        //            Uname = viewModel.Uname,
        //            Password = viewModel.Password,
        //            Oid = viewModel.Oid,
        //            Userlevel = int.Parse(viewModel.UserLevel),
        //            Uactive = viewModel.Uactive,
        //            Emailid = viewModel.EmailID
        //        };

        //        bool success = await _userManagementService.CreateUserAsync(user);

        //        if (success)
        //            return RedirectToAction(nameof(Index));
        //        else
        //            ModelState.AddModelError("", "An error occurred while creating the user.");
        //    }

        //    viewModel.UserLevelOptions = await _userManagementService.GetUserLevelOptionsAsync();
        //    return PartialView("_Create", viewModel);
        //}

        //public async Task<IActionResult> Edit(string id)
        //{
        //    var user = await _userManagementService.GetUserByIdAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    var viewModel = new UserManagementViewModel
        //    {
        //        UserID = user.Userid,
        //        Uname = user.Uname,
        //        Password = user.Password,
        //        Oid = user.Oid,
        //        UNO = user.Uno,
        //        UserLevel = user.Userlevel.ToString(),
        //        Uactive = user.Uactive,
        //        EmailID = user.Emailid,
        //        UserLevelOptions = await _userManagementService.GetUserLevelOptionsAsync()
        //    };

        //    return PartialView("_Edit", viewModel);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, UserManagementViewModel viewModel)
        //{
        //    if (id != viewModel.UserID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userManagementService.GetUserByIdAsync(id);
        //        if (user == null)
        //        {
        //            return NotFound();
        //        }

        //        user.Uname = viewModel.Uname;
        //        user.Password = viewModel.Password;
        //        user.Oid = viewModel.Oid;
        //        user.Userlevel = int.Parse(viewModel.UserLevel);
        //        user.Uactive = viewModel.Uactive;
        //        user.Emailid = viewModel.EmailID;

        //        bool success = await _userManagementService.UpdateUserAsync(user);

        //        if (success)
        //            return RedirectToAction(nameof(Index));
        //        else
        //            ModelState.AddModelError("", "An error occurred while updating the user.");
        //    }

        //    viewModel.UserLevelOptions = await _userManagementService.GetUserLevelOptionsAsync();
        //    // return View(viewModel);
        //    return PartialView("_Edit", viewModel);
        //}

        //public async Task<IActionResult> Delete(string id)
        //{
        //    var user = await _userManagementService.GetUserByIdAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(user);
        //}

        //// POST: User/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(string id)
        //{
        //    bool success = await _userManagementService.DeleteUserAsync(id);

        //    if (success)
        //        return RedirectToAction(nameof(Index));
        //    else
        //        return BadRequest("An error occurred while deleting the user.");
        //}


        //    public UserManagementController(IUserManagementService userManagementService, DbEcheckContext echeckContext)

        //    {
        //        _userManagementService = userManagementService;
        //        _EcheckContext = echeckContext;
        //    }
        //    public IActionResult Index()
        //    {
        //        var users = _userManagementService.GetUsers();
        //        return View(users);
        //    }

        //    public IActionResult Create()
        //    {
        //    ViewBag.Organizations = _userManagementService.GetOrganizations();

        //    ViewBag.UserLevels = new List<SelectListItem>
        //        {
        //            new SelectListItem { Value = "1", Text = "Admin" },
        //            new SelectListItem { Value = "2", Text = "SPOC" },
        //            new SelectListItem { Value = "3", Text = "Reports" },
        //            new SelectListItem { Value = "4", Text = "Data Entry User" },
        //            new SelectListItem { Value = "5", Text = "Data Viewer" }

        //        };

        //    return View();
        //}

        //    [HttpPost]

        //    public IActionResult Create(Ncuser user)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            bool Isadded = _userManagementService.AddUser(user);
        //            if (Isadded)

        //                return RedirectToAction("Index");
        //            else
        //                ModelState.AddModelError("", "UserID already exists.");

        //        }

        //        ViewBag.Organizations = _userManagementService.GetOrganizations();
        //        return View(user);
        //    }


        //    public IActionResult EditPartial(string userId)
        //    {
        //        if (string.IsNullOrEmpty(userId))
        //            return NotFound();

        //        var user = _userManagementService.GetUserById(userId);
        //        if (user == null)
        //            return NotFound();

        //        ViewBag.Organizations = _userManagementService.GetOrganizations();
        //        ViewBag.UserLevels = new List<SelectListItem>
        //{
        //    new SelectListItem { Value = "1", Text = "Admin" },
        //    new SelectListItem { Value = "2", Text = "SPOC" },
        //    new SelectListItem { Value = "3", Text = "Reports" },
        //    new SelectListItem { Value = "4", Text = "Data Entry User" },
        //    new SelectListItem { Value = "5", Text = "Data Viewer" }

        //};

        //        return PartialView("_EditPartial", user);
        //    }


        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public IActionResult Edit(Ncuser user)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            bool isUpdated = _userManagementService.UpdateUser(user);
        //            if (isUpdated)
        //            {
        //                return RedirectToAction("Index");
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("", "Error updating user.");
        //            }
        //        }

        //        ViewBag.Organizations = _userManagementService.GetOrganizations();
        //        ViewBag.UserLevels = new List<SelectListItem>
        //    {
        //        new SelectListItem { Value = "1", Text = "Admin" },
        //        new SelectListItem { Value = "2", Text = "SPOC" },
        //        new SelectListItem { Value = "3", Text = "Reports" },
        //        new SelectListItem { Value = "4", Text = "Data Entry User" },
        //        new SelectListItem { Value = "5", Text = "Data Viewer" }

        //    };

        //           return View(user);


        //    }

        //    private Dictionary<int, string> _userLevelNames = new Dictionary<int, string>
        //        {
        //            { 1, "Reports" },
        //            { 5, "Uploader" },
        //            { 10, "Auditor" },
        //            { 15, "Owner" },
        //            { 101, "Contribution" },
        //            { 102, "Registration" },    
        //            { 103, "Return" },
        //            { 104, "Registration and Return" },
        //            { 105, "All 3" }
        //        };
        //    public IActionResult MapUser(string userId)
        //    {

        //       var user = _EcheckContext.Ncusers.FirstOrDefault(u => u.Userid == userId);
        //        if (user == null) return NotFound();




        //    ViewBag.UserId = user.Userid;

        //        // Fetch the organizations authorized to the user
        //        var authorizedMappings = (from mapping in _EcheckContext.Ncumaps
        //                                  join org in _EcheckContext.Ncmorgs on mapping.Oid equals org.Oid
        //                                  join loc in _EcheckContext.Ncmlocs on mapping.Lcode equals loc.Lcode
        //                                  where mapping.Uno == user.Uno
        //                                  select new
        //                                  {
        //                                      mapping.Oid,
        //                                      org.Oname,
        //                                      loc.Lcode,
        //                                      loc.Lname,
        //                                      mapping.Ulevel
        //                                  })
        //                           .GroupBy(m => new { m.Oid, m.Oname })
        //                           .Select(g => new
        //                           {
        //                               g.Key.Oid,
        //                               g.Key.Oname,
        //                               Locations = g.Select(l => new
        //                               {
        //                                   l.Lcode,
        //                                   l.Lname,
        //                                   l.Ulevel
        //                               }).ToList()
        //                           }).ToList();

        //        ViewBag.AuthorizedMappings = authorizedMappings;

        //        // Set user levels based on the user's current level
        //        if (user.Userlevel == 2 || user.Userlevel == 3)
        //        {
        //            ViewBag.UserLevels = new List<SelectListItem>
        //    {
        //        new SelectListItem { Value = "1", Text = "Reports" },
        //        new SelectListItem { Value = "5", Text = "Uploader" },
        //        new SelectListItem { Value = "10", Text = "Auditor" },
        //        new SelectListItem { Value = "15", Text = "Owner" }
        //    };
        //        }
        //        else if (user.Userlevel == 4 || user.Userlevel == 5)
        //        {
        //            ViewBag.UserLevels = new List<SelectListItem>
        //    {
        //        new SelectListItem { Value = "101", Text = "Contribution" },
        //        new SelectListItem { Value = "102", Text = "Registration" },
        //        new SelectListItem { Value = "103", Text = "Return" },
        //        new SelectListItem { Value = "104", Text = "Regs and Return" },
        //        new SelectListItem { Value = "105", Text = "All 3" }
        //    };
        //        }

        //        else
        //        {
        //            {

        //                ViewBag.UserLevels = new List<SelectListItem>(); // No options for other levels  
        //            }

        //        }

        //        ViewBag.UserLevelNames = _userLevelNames;
        //        ViewBag.Organizations = _userManagementService.GetOrganizations();


        //        return View();
        //    }


        //    [HttpPost]
        //    public IActionResult MapUser(string userId, Ncumap mapping)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var user = _userManagementService.GetUserById(userId);
        //            if (user != null)
        //            {
        //                mapping.Uno = user.Uno;

        //                // Set the Ulevel based on the selected value
        //                if (mapping.Ulevel == "1" || mapping.Ulevel == "5" || mapping.Ulevel == "10" || mapping.Ulevel == "15")
        //                {
        //                    // Map to the corresponding values for Reports, Uploader, Auditor, Owner
        //                    mapping.Ulevel = mapping.Ulevel; // This will be 1, 5, 10, or 15
        //                }
        //                else if (mapping.Ulevel == "101" || mapping.Ulevel == "102" || mapping.Ulevel == "103" || mapping.Ulevel == "104" || mapping.Ulevel == "105")
        //                {
        //                    // Map to the corresponding values for Contribution, Registration, Return, Regs and Return, All 3
        //                    mapping.Ulevel = mapping.Ulevel; // This will be 101, 102, 103, 104, or 105
        //                }

        //                bool isMapped = _userManagementService.MapUserToOrgLocation(mapping);
        //                if (isMapped)
        //                    return RedirectToAction("Index", "UserManagement");

        //                ModelState.AddModelError("", "Mapping already exists!");
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("", "User not found!");
        //            }

        //        }

        //        ViewBag.Organizations = _userManagementService.GetOrganizations();


        //        return View(mapping);
        //    }

        //    [HttpPost]
        //    public IActionResult UpdateUserLevel(int Uno, string userId, string lcode, string newLevel)
        //    {
        //        var mapping = _EcheckContext.Ncumaps.FirstOrDefault(m => m.Uno == Uno && m.Lcode == lcode);
        //        if (mapping == null)
        //        {
        //            return NotFound();
        //        }

        //        mapping.Ulevel = newLevel;
        //        _EcheckContext.SaveChanges();

        //        return Json(new { success = true });
        //    }


        //    public JsonResult GetLocations(string oid)
        //    {
        //        var locations = _userManagementService.GetLocationsByOrg(oid);
        //        return Json(locations);
        //    }



    }

}