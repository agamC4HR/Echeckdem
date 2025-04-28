using Microsoft.AspNetCore.Mvc.Rendering;
using Echeckdem.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Bibliography;
using Echeckdem.CustomFolder;
using Echeckdem.CustomFolder.UserManagement;

namespace Echeckdem.Services
{
    public interface IUserManagementService
    {
        Task<List<UserManagementViewModel>> GetAllUsersAsync();

        Task AddUserAsync(UserCreateViewModel model);
        Task<List<OrganisationViewModel>> GetAllOrganisationsAsync();
        Task<List<UserMappingViewModel>> GetUserMappingAsync(string userId);
        //Task<Ncuser> GetUserByIdAsync(string userId);
        //Task<string> GetOrganizationNameByOidAsync(string oid);
        //Task<List<SelectListItem>> GetUserLevelOptionsAsync();
        //Task<bool> CreateUserAsync(Ncuser user);
        //Task<bool> UpdateUserAsync(Ncuser user);
        //Task<bool> DeleteUserAsync(string userId);
    }

    public class UserManagementService : IUserManagementService
    {
        private readonly DbEcheckContext _EcheckContext;
        private readonly ILogger<UserManagementService> _logger;

        public UserManagementService(DbEcheckContext echeckContext, ILogger<UserManagementService> logger)
        {
            _EcheckContext = echeckContext;
            _logger = logger;
        }

        public async Task<List<UserManagementViewModel>> GetAllUsersAsync()
        {
            // Step 1: Fetch the data from the database
            var users = await (from user in _EcheckContext.Ncusers
                               join org in _EcheckContext.Ncmorgs on user.Oid equals org.Oid into userOrg
                               from org in userOrg.DefaultIfEmpty()
                               select new
                               {
                                   user.Userid,
                                   user.Uname,
                                   user.Emailid,
                                   user.Userlevel,
                                   OrganisationName = org != null ? org.Oname : "No Organisation"
                               }).ToListAsync();

            // Step 2: Map the user data to UserManagementViewModel
            var userViewModels = users.Select(user => new UserManagementViewModel
            {
                UserID = user.Userid,
                UNAME = user.Uname,
                EmailID = user.Emailid,
                UserLevel = GetUserLevelName(user.Userlevel.GetValueOrDefault()), // Handle user level here
                OrganisationName = user.OrganisationName
            }).ToList();

            return userViewModels;
        }

        public async Task AddUserAsync(UserCreateViewModel model)
        {
            var newUser = new Ncuser
            {
                Userid = model.UserID,
                Uname = model.UNAME,
                Password = model.Password,
                Oid = model.OID,
                Userlevel = model.UserLevel,
                Uactive = 1, // Assume active by default
                Emailid = model.EmailID
            };

            _EcheckContext.Ncusers.Add(newUser);
            await _EcheckContext.SaveChangesAsync();
        }

        private string GetUserLevelName(int level)
        {
            return level switch
            {
                1 => "Admin",
                2 => "SPOC",
                3 => "Reports",
                4 => "Data Entry User",
                5 => "Data Viewer",
                _ => "Unknown"
            };
        }

        private string GetMappingUserLevelName(int level)
        {
            return level switch
            {
                1 => "Reports",
                5 => "Uploader",
                10 => "Auditor",
                15 => "Owner",
                101 => "Contributions",
                102 => "Registrations",
                103 => "Returns",
                104 => "Registrations and Contributions",
                105 => "All 3",
                _ => "Unknown"
            };
        }


        public async Task<List<OrganisationViewModel>> GetAllOrganisationsAsync()
        {
            return await _EcheckContext.Ncmorgs
                                      .Select(org => new OrganisationViewModel
                                      {
                                          OID = org.Oid,
                                          OrganisationName = org.Oname
                                      })
                                      .ToListAsync();
        }

        public async Task<List<UserMappingViewModel>> GetUserMappingAsync(string userId)
        {
            _logger.LogInformation("Starting GetUserMappingAsync for userId: {UserId}", userId);
            var user = await _EcheckContext.Ncusers.Where(u=>u.Userid == userId).FirstOrDefaultAsync();

            if (user == null)
            {
                _logger.LogWarning("User with UserId: {UserId} not found.", userId);
                throw new Exception("User not found.");
            }

            var uno = user.Uno;
            _logger.LogInformation("User found. Fetching mappings for Uno: {Uno}", uno);

            var rawMappings = await (from map in _EcheckContext.Ncumaps
                                  join org in _EcheckContext.Ncmorgs on map.Oid equals org.Oid
                                  join loc in _EcheckContext.Ncmlocs on map.Lcode equals loc.Lcode
                                  where map.Uno == uno
                                     select new
                                     {
                                         org.Oname,
                                         loc.Lname,
                                         map.Ulevel
                                     }).ToListAsync();

            _logger.LogInformation("Mappings retrieved for Uno: {Uno}. Processing user levels...", uno);

            var mappings = rawMappings.Select(m =>
            {
                // Safely try to convert Ulevel to int
                int parsedUlevel = 0; // Default value
                if (!string.IsNullOrEmpty(m.Ulevel) && int.TryParse(m.Ulevel, out parsedUlevel))
                {
                    _logger.LogInformation("Successfully parsed UserLevel: {UserLevel} to integer", m.Ulevel);
                }
                else
                {
                    _logger.LogWarning("Failed to parse UserLevel: {UserLevel}, using default value: {DefaultUserLevel}", m.Ulevel, parsedUlevel);
                }

                return new UserMappingViewModel
                {
                    OrganisationName = m.Oname,
                    LocationName = m.Lname,
                    UserLevel = GetMappingUserLevelName(parsedUlevel) // Use parsed int value
                };
            }).ToList();

            _logger.LogInformation("Finished processing mappings for Uno: {Uno}. Total mappings: {MappingsCount}", uno, mappings.Count);


            return mappings;



        }




























        //public async Task<List<Ncuser>> GetAllUsersAsync()
        //{   
        //    return await _EcheckContext.Ncusers.ToListAsync();
        //}

        //public async Task<Ncuser> GetUserByIdAsync(string userId)
        //{
        //    return await _EcheckContext.Ncusers.FindAsync(userId);
        //}

        //public async Task<string> GetOrganizationNameByOidAsync(string oid)
        //{
        //    var org = await _EcheckContext.Ncmorgs.FirstOrDefaultAsync(o => o.Oid == oid);
        //    return org?.Oname ?? "Unknown Organization";
        //}

        //public async Task<List<SelectListItem>> GetUserLevelOptionsAsync()
        //{
        //    return new List<SelectListItem>
        //{
        //    new SelectListItem { Text = "Admin", Value = "1" },
        //    new SelectListItem { Text = "Spoc", Value = "2" },
        //    new SelectListItem { Text = "Reports", Value = "3" },
        //    new SelectListItem { Text = "Data Entry User", Value = "4" }, 
        //    new SelectListItem { Text = "Data Viewer", Value = "5" }
        //};
        //}

        //public async Task<bool> CreateUserAsync(Ncuser user)
        //{
        //    try
        //    {
        //        _EcheckContext.Ncusers.Add(user);
        //        await _EcheckContext.SaveChangesAsync();
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //public async Task<bool> UpdateUserAsync(Ncuser user)
        //{
        //    try
        //    {
        //        _EcheckContext.Ncusers.Update(user);
        //        await _EcheckContext.SaveChangesAsync();
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //public async Task<bool> DeleteUserAsync(string userId)
        //{
        //    try
        //    {
        //        var user = await GetUserByIdAsync(userId);
        //        if (user != null)
        //        {
        //            _EcheckContext.Ncusers.Remove(user);
        //            await _EcheckContext.SaveChangesAsync();
        //            return true;
        //        }
        //        return false;
        //    }
        //    catch
        //    {
        //        return false;
        //    }

        //}

































        //    private readonly Dictionary<int, string> _userLevels = new Dictionary<int, string>
        //{
        //    { 1, "Admin" },
        //    { 2, "SPOC" },
        //    { 3, "Reports" },
        //    { 4, "Data Entry User" },
        //    { 5, "Data Viewer" }

        //};

        //    public List<Ncuser> GetUsers()
        //    {
        //        return (from u in _EcheckContext.Ncusers
        //                join o in _EcheckContext.Ncmorgs on u.Oid equals o.Oid
        //                select new Ncuser
        //                {

        //                    Userid = u.Userid,
        //                    Uname = u.Uname,
        //                    Userlevel = u.Userlevel,
        //                    Uactive = u.Uactive,
        //                    Oid = u.Oid,
        //                    OName = o.Oname,
        //                    UserLevelName = _userLevels.ContainsKey(u.Userlevel ?? 0) ? _userLevels[u.Userlevel ?? 0] : "Unknown"
        //                }).ToList();


        //    }





        //    public bool AddUser(Ncuser user)
        //    {
        //        try
        //        {


        //            if (!_EcheckContext.Ncusers.Any(u => u.Userid == user.Userid))
        //            {
        //                user.Uactive = 1;
        //                _EcheckContext.Ncusers.Add(user);
        //                _EcheckContext.SaveChanges();
        //                return true;
        //            }

        //            return false;
        //        }

        //        catch (Exception ex)
        //        {
        //            throw new ApplicationException("An error occurred while adding the user.", ex);
        //        }
        //    }

        //    public List<SelectListItem> GetOrganizations()
        //    {
        //        return _EcheckContext.Ncmorgs
        //            .Select(o => new SelectListItem
        //            {
        //                Value = o.Oid.ToString(),
        //                Text = o.Oname
        //            }).ToList();
        //    }


        //    public Ncuser GetUserById(string userId)
        //    {
        //        return _EcheckContext.Ncusers
        //            .Where(u => u.Userid == userId)
        //            .Select(u => new Ncuser
        //            {
        //                Uno = u.Uno,
        //                Userid = u.Userid,
        //                Uname = u.Uname,
        //                Userlevel = u.Userlevel,
        //                Uactive = u.Uactive,
        //                Oid = u.Oid
        //            })
        //            .FirstOrDefault();
        //    }

        //    public List<SelectListItem> GetLocationsByOrg(string oid)
        //    {
        //        return _EcheckContext.Ncmlocs.Where(l => l.Oid == oid && l.Lactive == 1)
        //            .Select(l => new SelectListItem
        //            {
        //                Value = l.Lcode,
        //                Text = $"{l.Lname} - {l.Lcity} - {l.Lstate} - {l.Lregion}" // You can modify this to display location names if available
        //            }).ToList();
        //    }

        //    public bool MapUserToOrgLocation(Ncumap mapping)
        //    {
        //        if (!_EcheckContext.Ncumaps.Any(m => m.Uno == mapping.Uno && m.Lcode == mapping.Lcode))
        //        {
        //            _EcheckContext.Ncumaps.Add(mapping);
        //            _EcheckContext.SaveChanges();
        //            return true;
        //        }
        //        return false;
        //    }

        //    public bool UpdateUser(Ncuser updatedUser)
        //    {
        //        var existingUser = _EcheckContext.Ncusers.FirstOrDefault(u => u.Userid == updatedUser.Userid);
        //        if (existingUser != null)
        //        {
        //            existingUser.Uname = updatedUser.Uname;
        //            existingUser.Userlevel = updatedUser.Userlevel;
        //            existingUser.Uactive = updatedUser.Uactive;
        //            existingUser.Oid = updatedUser.Oid;

        //            _EcheckContext.SaveChanges();
        //            return true;

        //        }
        //        return false;

        //    }

    }
}


