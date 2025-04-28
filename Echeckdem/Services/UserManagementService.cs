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

        Task<List<LocationViewModel>> GetLocationsByOrganisationAsync(string organisationOid);

        Task<UserMappingCreationDataViewModel> GetUserMappingCreationDataAsync(string userId);

        Task AddUserMappingAsync(UserMappingCreationModel model);


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

        public async Task<List<LocationViewModel>> GetLocationsByOrganisationAsync(string oid)
        {
            // Fetch locations where organisation oid matches
            var locations = await _EcheckContext.Ncmlocs
                                                .Where(loc => loc.Oid == oid)
                                                .Select(loc => new LocationViewModel
                                                {
                                                    Lcode = loc.Lcode,
                                                    Lname = loc.Lname
                                                })
                                                .ToListAsync();

            return locations;
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

        public async Task<List<UserMappingViewModel>> GetUserMappingAsync(string userId)
        {
            _logger.LogInformation("Starting GetUserMappingAsync for userId: {UserId}", userId);
            var user = await _EcheckContext.Ncusers.Where(u => u.Userid == userId).FirstOrDefaultAsync();

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

        public async Task<UserMappingCreationDataViewModel> GetUserMappingCreationDataAsync(string userId)
        {
            var user = await _EcheckContext.Ncusers.Where(u => u.Userid == userId).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var organisations = await GetAllOrganisationsAsync();

            return new UserMappingCreationDataViewModel
            {
                UserId = user.Userid,
                Uno = user.Uno,
                UserLevel = user.Userlevel,
                Organisations = organisations
            };
        }

        public async Task AddUserMappingAsync(UserMappingCreationModel model)
        {
            var newUserMap = new Ncumap
            {
                Uno = model.Uno,
                Oid = model.Oid,
                Lcode = model.Lcode,
                Ulevel = model.SelectedUserLevel.ToString() // Store the integer value as string
            };

            _EcheckContext.Ncumaps.Add(newUserMap);
            await _EcheckContext.SaveChangesAsync();
        }
    }

}

