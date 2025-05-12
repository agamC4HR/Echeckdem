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

        // ----------------START-------------------------------VIEW ALL USERS---------------------------------------------------------------------------------------
        public async Task<IActionResult> Index()
        {
            var users = await _userManagementService.GetAllUsersAsync();
            return View(users);
        }
        // ----------------END-------------------------------VIEW ALL USERS---------------------------------------------------------------------------------------
        // ----------------START-------------------------------Adding Users/ Populating NCMLOC USERS---------------------------------------------------------------------------------------
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
        [HttpGet]
        public async Task<IActionResult> GetUserAvailable(string userid)
        {
            var useravailable = await _EcheckContext.Ncusers.Where(u=>u.Userid.Trim()==userid.Trim()).FirstOrDefaultAsync();
            return Content(useravailable == null ? "true" : "false");
        }
        [HttpGet]
        public async Task<IActionResult> GetEmailAvailable(string email)
        {
            var emailavailable = await _EcheckContext.Ncusers.Where(u => u.Emailid.Trim() == email.Trim()).FirstOrDefaultAsync();
            return Content(emailavailable == null ? "true" : "false");
        }

        // ----------------EnD-------------------------------Adding Users/ Populating NCMLOC USERS---------------------------------------------------------------------------------------
        //------------------------START--------------------------Editing the data for user/ Editing NCMLOC------------------------------------------------------------------------------------------------
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var user = await _userManagementService.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            await PopulateDropDownsAsync(user);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _userManagementService.UpdateUserAsync(model);
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropDownsAsync(model);
            return View(model);
        }

        private async Task PopulateDropDownsAsync(UserCreateViewModel model = null)
        {
            var organisations = await _userManagementService.GetAllOrganisationsAsync();
            ViewBag.Organisations = new SelectList(organisations, "OID", "OrganisationName", model?.OID);

            var userLevels = _userManagementService.GetUserLevelOptions();
            ViewBag.UserLevels = new SelectList(userLevels, "Key", "Value", model?.UserLevel);

            var activeOptions = new[]
            {
                new SelectListItem { Text = "Yes", Value = "1", Selected = model != null && model.Uactive == 1 },
                new SelectListItem { Text = "No", Value = "0", Selected = model != null && model.Uactive == 0 }
            };

            ViewBag.ActiveOptions = activeOptions;
        }

        //------------------------END--------------------------Editing the data for user/ Editing NCMLOC------------------------------------------------------------------------------------------------

        [HttpPost]
        public IActionResult MapOrganisation(string userId)
        {
           
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }
            
            HttpContext.Session.SetString("UserId", userId);

            return RedirectToAction("MapOrganisation");
        }


        //[HttpGet]
        //public async Task<IActionResult> MapOrganisation()        // yo h

        //{
        //    string userId = HttpContext.Session.GetString("UserId");
        //    //string userId = HttpContext.Session.GetString("UserId"); 


        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        return BadRequest("failed to get userid");
        //    }

        //    try
        //    {
        //        var mappings = await _userManagementService.GetUserMappingAsync(userId);
        //        ViewBag.UserId = userId;
        //        return View(mappings);
        //    }
        //    catch (Exception ex)
        //    {

        //        return NotFound(ex.Message);
        //    }
        //}
        [HttpGet]
        public async Task<IActionResult> MapOrganisation()        // yo h
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

                // Fetch User's base UserLevel to determine edit dropdown options
                var user = await _EcheckContext.Ncusers.Where(u => u.Userid == userId).FirstOrDefaultAsync();
                if (user != null)
                {
                    // Check if user.Userlevel has a value before passing it
                    if (user.Userlevel.HasValue)
                    {
                        ViewBag.UserLevelOptions = GetMappingUserLevelOptions(user.Userlevel.Value);
                    }
                    else
                    {
                        ViewBag.UserLevelOptions = new List<SelectListItem>(); // Or handle the case where UserLevel is null
                    }
                }
                else
                {
                    ViewBag.UserLevelOptions = new List<SelectListItem>();
                }

                return View(mappings);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpPost]
        public async Task<JsonResult> GetLocationsByOrganisation(string oid)
        {
            if (string.IsNullOrEmpty(oid))
            {
                return Json(new { success = false, message = "Organisation ID is required." });
            }

            var locations = await _userManagementService.GetLocationsByOrganisationAsync(oid);
            return Json(new { success = true, data = locations });
        }





        [HttpPost]
        public IActionResult InitiateAddMapping(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            // Store userId in TempData to pass to the next action (redirect)
            TempData["UserIdForMapping"] = userId;
            return RedirectToAction(nameof(AddUserMapping));
        }

        [HttpGet]
        public async Task<IActionResult> AddUserMapping()   // yo h
        {
            string userId = TempData["UserIdForMapping"] as string;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID not found.");
            }

            try
            {
                var creationData = await _userManagementService.GetUserMappingCreationDataAsync(userId);
                ViewBag.UserId = creationData.UserId;
                ViewBag.Uno = creationData.Uno;

                List<SelectListItem> userLevelOptions = new List<SelectListItem>();

                if (creationData.UserLevel == 0 || creationData.UserLevel == 1)           // NOT SURE ABOUT THIS
                {
                    userLevelOptions.Add(new SelectListItem { Text = "Reports", Value = "1" });
                    userLevelOptions.Add(new SelectListItem { Text = "Uploader", Value = "5" });
                    userLevelOptions.Add(new SelectListItem { Text = "Auditor", Value = "10" });
                    userLevelOptions.Add(new SelectListItem { Text = "Owner", Value = "15" });
                    userLevelOptions.Add(new SelectListItem { Text = "Contributions", Value = "101" });
                    userLevelOptions.Add(new SelectListItem { Text = "Registrations", Value = "102" });
                    userLevelOptions.Add(new SelectListItem { Text = "Returns", Value = "103" });
                    userLevelOptions.Add(new SelectListItem { Text = "Registrations and Returns", Value = "104" });
                    userLevelOptions.Add(new SelectListItem { Text = "All 3", Value = "105" });
                }
                else if (creationData.UserLevel == 2 || creationData.UserLevel == 3)
                {
                    userLevelOptions.Add(new SelectListItem { Text = "Reports", Value = "1" });
                    userLevelOptions.Add(new SelectListItem { Text = "Uploader", Value = "5" });
                    userLevelOptions.Add(new SelectListItem { Text = "Auditor", Value = "10" });
                    userLevelOptions.Add(new SelectListItem { Text = "Owner", Value = "15" });
                }
                else if (creationData.UserLevel == 4 || creationData.UserLevel == 5)
                {
                    userLevelOptions.Add(new SelectListItem { Text = "Contributions", Value = "101" });
                    userLevelOptions.Add(new SelectListItem { Text = "Registrations", Value = "102" });
                    userLevelOptions.Add(new SelectListItem { Text = "Returns", Value = "103" });
                    userLevelOptions.Add(new SelectListItem { Text = "Registrations and Returns", Value = "104" });
                    userLevelOptions.Add(new SelectListItem { Text = "All 3", Value = "105" });
                }
                ViewBag.UserLevelOptions = userLevelOptions;

                ViewBag.Organisations = new SelectList(creationData.Organisations, "OID", "OrganisationName");
                ViewBag.Locations = new List<SelectListItem>(); // Will be populated via AJAX

                return View();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetLocations(string oid)
        {
            if (string.IsNullOrEmpty(oid))
            {
                return BadRequest("Organisation ID is required.");
            }

            var locations = await _userManagementService.GetLocationsByOrganisationAsync(oid);
            return Json(locations.Select(l => new { id = l.Lcode, name = l.Lname }));
        }

        [HttpPost]
        public async Task<IActionResult> AddUserMapping(UserMappingCreationModel model)
        {
            if (ModelState.IsValid)
            {
                await _userManagementService.AddUserMappingAsync(model);
                return RedirectToAction(nameof(Index)); // Redirect to user list after adding mapping
            }

            // If model is not valid, we need to repopulate the ViewBag for the view
            string userId = await _EcheckContext.Ncusers.Where(u => u.Uno == model.Uno).Select(u => u.Userid).FirstOrDefaultAsync();
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Invalid User Uno.");
            }
            var creationData = await _userManagementService.GetUserMappingCreationDataAsync(userId);
            ViewBag.UserId = creationData.UserId;
            ViewBag.Uno = creationData.Uno;

            List<SelectListItem> userLevelOptions = new List<SelectListItem>();
            if (creationData.UserLevel == 2 || creationData.UserLevel == 3)
            {
                userLevelOptions.Add(new SelectListItem { Text = "Reports", Value = "1" });
                userLevelOptions.Add(new SelectListItem { Text = "Uploader", Value = "5" });
                userLevelOptions.Add(new SelectListItem { Text = "Auditor", Value = "10" });
                userLevelOptions.Add(new SelectListItem { Text = "Owner", Value = "15" });
            }
            else if (creationData.UserLevel == 4 || creationData.UserLevel == 5)
            {
                userLevelOptions.Add(new SelectListItem { Text = "Contributions", Value = "101" });
                userLevelOptions.Add(new SelectListItem { Text = "Registrations", Value = "102" });
                userLevelOptions.Add(new SelectListItem { Text = "Returns", Value = "103" });
                userLevelOptions.Add(new SelectListItem { Text = "Registrations and Returns", Value = "104" });
                userLevelOptions.Add(new SelectListItem { Text = "All 3", Value = "105" });
            }
            ViewBag.UserLevelOptions = userLevelOptions;
            ViewBag.Organisations = new SelectList(creationData.Organisations, "OID", "OrganisationName", model.Oid);

            
            var locations = await _userManagementService.GetLocationsByOrganisationAsync(model.Oid);
            ViewBag.Locations = new SelectList(locations, "Lcode", "Lname", model.Lcode);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditUserMapping(string organisationName, string locationName, string newUserLevel)
        {
            string userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID not found in session.");
            }

            var user = await _EcheckContext.Ncusers.Where(u => u.Userid == userId).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound($"User with ID '{userId}' not found.");
            }

            var uno = user.Uno;

            var organisation = await _EcheckContext.Ncmorgs.FirstOrDefaultAsync(o => o.Oname == organisationName);
            var location = await _EcheckContext.Ncmlocs.FirstOrDefaultAsync(l => l.Lname == locationName);

            if (organisation == null || location == null)
            {
                return NotFound("Organisation or Location not found.");
            }

            var mappingToUpdate = await _EcheckContext.Ncumaps.FirstOrDefaultAsync(m => m.Uno == uno && m.Oid.Trim() == organisation.Oid.Trim() &&  m.Lcode.Trim() == location.Lcode.Trim());

            if (mappingToUpdate == null)
            {
                return NotFound("Mapping not found.");
            }

            mappingToUpdate.Ulevel = newUserLevel; // Update with the new user level
            await _EcheckContext.SaveChangesAsync();

            return RedirectToAction("MapOrganisation");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserMapping(string organisationName, string locationName)
        {
            string userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID not found in session.");
            }

            var user = await _EcheckContext.Ncusers.Where(u => u.Userid == userId).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound($"User with ID '{userId}' not found.");
            }

            var uno = user.Uno;

            var organisation = await _EcheckContext.Ncmorgs.FirstOrDefaultAsync(o => o.Oname == organisationName);
            var location = await _EcheckContext.Ncmlocs.FirstOrDefaultAsync(l => l.Lname == locationName);

            if (organisation == null || location == null)
            {
                return NotFound("Organisation or Location not found.");
            }

            var mappingToDelete = await _EcheckContext.Ncumaps.FirstOrDefaultAsync(m => m.Uno == uno && m.Oid == organisation.Oid && m.Lcode == location.Lcode);

            if (mappingToDelete == null)
            {
                return NotFound("Mapping not found.");
            }

            _EcheckContext.Ncumaps.Remove(mappingToDelete);
            await _EcheckContext.SaveChangesAsync();

            return RedirectToAction("MapOrganisation");
        }

        private List<SelectListItem> GetMappingUserLevelOptions(int userLevel)
        {
            List<SelectListItem> userLevelOptions = new List<SelectListItem>();

            if (userLevel == 0 || userLevel == 1)
            {
                userLevelOptions.Add(new SelectListItem { Text = "Reports", Value = "1" });
                userLevelOptions.Add(new SelectListItem { Text = "Uploader", Value = "5" });
                userLevelOptions.Add(new SelectListItem { Text = "Auditor", Value = "10" });
                userLevelOptions.Add(new SelectListItem { Text = "Owner", Value = "15" });
                userLevelOptions.Add(new SelectListItem { Text = "Contributions", Value = "101" });
                userLevelOptions.Add(new SelectListItem { Text = "Registrations", Value = "102" });
                userLevelOptions.Add(new SelectListItem { Text = "Returns", Value = "103" });
                userLevelOptions.Add(new SelectListItem { Text = "Registrations and Returns", Value = "104" });
                userLevelOptions.Add(new SelectListItem { Text = "All 3", Value = "105" });
            }
            else if (userLevel == 2 || userLevel == 3)
            {
                userLevelOptions.Add(new SelectListItem { Text = "Reports", Value = "1" });
                userLevelOptions.Add(new SelectListItem { Text = "Uploader", Value = "5" });
                userLevelOptions.Add(new SelectListItem { Text = "Auditor", Value = "10" });
                userLevelOptions.Add(new SelectListItem { Text = "Owner", Value = "15" });
            }
            else if (userLevel == 4 || userLevel == 5)
            {
                userLevelOptions.Add(new SelectListItem { Text = "Contributions", Value = "101" });
                userLevelOptions.Add(new SelectListItem { Text = "Registrations", Value = "102" });
                userLevelOptions.Add(new SelectListItem { Text = "Returns", Value = "103" });
                userLevelOptions.Add(new SelectListItem { Text = "Registrations and Returns", Value = "104" });
                userLevelOptions.Add(new SelectListItem { Text = "All 3", Value = "105" });
            }
            return userLevelOptions;
        }



    }

}