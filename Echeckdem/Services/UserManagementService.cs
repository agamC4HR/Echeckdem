using Microsoft.AspNetCore.Mvc.Rendering;
using Echeckdem.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;

namespace Echeckdem.Services
{
    public interface IUserManagementService
    {
        List<Ncuser> GetUsers();
        bool AddUser(Ncuser user);
        List<SelectListItem> GetOrganizations();
        List<SelectListItem> GetLocationsByOrg(string oid);
        bool MapUserToOrgLocation(Ncumap mapping);
        Ncuser GetUserById(string userId);

        bool UpdateUser(Ncuser updatedUser);
    }

    public class UserManagementService : IUserManagementService
    {
        private readonly DbEcheckContext _EcheckContext;

        public UserManagementService(DbEcheckContext echeckContext)
        {
            _EcheckContext = echeckContext;
        }

        private readonly Dictionary<int, string> _userLevels = new Dictionary<int, string>
    {
        { 1, "Admin" },
        { 2, "SPOC" },
        { 3, "Reports" },
        { 4, "Data Entry User" },
        { 5, "Data Viewer" }
        
    };

        public List<Ncuser> GetUsers()
        {
            return (from u in _EcheckContext.Ncusers
                    join o in _EcheckContext.Ncmorgs on u.Oid equals o.Oid
                    select new Ncuser
                    {

                        Userid = u.Userid,
                        Uname = u.Uname,
                        Userlevel = u.Userlevel,
                        Uactive = u.Uactive,
                        Oid = u.Oid,
                        OName = o.Oname,
                        UserLevelName = _userLevels.ContainsKey(u.Userlevel ?? 0) ? _userLevels[u.Userlevel ?? 0] : "Unknown"
                    }).ToList();


        }





        public bool AddUser(Ncuser user)
        {
            if (!_EcheckContext.Ncusers.Any(u => u.Userid == user.Userid))
            {
                user.Uactive = 1;
                _EcheckContext.Ncusers.Add(user);
                _EcheckContext.SaveChanges();
                return true;
            }

            return false;
        }

        public List<SelectListItem> GetOrganizations()
        {
            return _EcheckContext.Ncmorgs
                .Select(o => new SelectListItem
                {
                    Value = o.Oid.ToString(),
                    Text = o.Oname
                }).ToList();
        }

       
        public Ncuser GetUserById(string userId)
        {
            return _EcheckContext.Ncusers
                .Where(u => u.Userid == userId)
                .Select(u => new Ncuser
                {
                    Uno = u.Uno,  
                    Userid = u.Userid,
                    Uname = u.Uname,
                    Userlevel = u.Userlevel,
                    Uactive = u.Uactive,
                    Oid = u.Oid
                })
                .FirstOrDefault();
        }

        public List<SelectListItem> GetLocationsByOrg(string oid)
        {
            return _EcheckContext.Ncmlocs.Where(l => l.Oid == oid && l.Lactive == 1)
                .Select(l => new SelectListItem
                {
                    Value = l.Lcode,
                    Text = $"{l.Lname} - {l.Lcity} - {l.Lstate} - {l.Lregion}" // You can modify this to display location names if available
                }).ToList();
        }

        public bool MapUserToOrgLocation(Ncumap mapping)
        {
            if (!_EcheckContext.Ncumaps.Any(m => m.Uno == mapping.Uno && m.Lcode == mapping.Lcode))
            {
                _EcheckContext.Ncumaps.Add(mapping);
                _EcheckContext.SaveChanges();
                return true;
            }
            return false;
        }

        public bool UpdateUser(Ncuser updatedUser)
        {
            var existingUser = _EcheckContext.Ncusers.FirstOrDefault(u => u.Userid == updatedUser.Userid);
            if (existingUser != null)
            {
                existingUser.Uname = updatedUser.Uname;
                existingUser.Userlevel = updatedUser.Userlevel;
                existingUser.Uactive = updatedUser.Uactive;
                existingUser.Oid = updatedUser.Oid;

                _EcheckContext.SaveChanges();
                return true;

            }
            return false;

        }

    }
}
