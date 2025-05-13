using Echeckdem.Models;
using MongoDB.Driver;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Identity;
using Echeckdem.CustomFolder;
using DocumentFormat.OpenXml.Spreadsheet;
using Echeckdem.CustomFolder.UserManagement;

namespace Echeckdem.Services
{
    public class UserService : IUserService
    {

        private readonly DbEcheckContext _dbEcheckContext;
        private readonly PasswordHasher<Ncuser> _passwordHasher = new();

        public UserService(DbEcheckContext dbEcheckContext)
        {
            _dbEcheckContext = dbEcheckContext;
        }

        public bool IsValidUser(string Userid, string Password)
        {
            var user = _dbEcheckContext.Ncusers.Where(u => u.Userid == Userid && u.Password == Password).FirstOrDefault();

            return user != null;
        }
        public bool IsValidUserhash(string Userid, string Password)
        {
            //var admin = _dbEcheckContext.Ncusers.FirstOrDefault(u => u.Userid == "ladoo");
            //admin.HashPassword = _passwordHasher.HashPassword(admin, "123");
            //_dbEcheckContext.SaveChanges();
            var user = _dbEcheckContext.Ncusers.FirstOrDefault(u => u.Userid == Userid);
            if (user == null) return false;
            if(user.HashPassword==null) return false;

            var result = _passwordHasher.VerifyHashedPassword(user, user.HashPassword, Password);
            return result == PasswordVerificationResult.Success;
        }
        public async Task<int> GetUserLevelAsync(string userId)
        {
            var user = await _dbEcheckContext.Ncusers
                .Where(u => u.Userid == userId)
                .FirstOrDefaultAsync();

            return user?.Userlevel ?? 0;
        }

        public async Task<int> GetUserUnoAsync(string userId)
        {
            var user = await _dbEcheckContext.Ncusers
                .Where (u => u.Userid == userId)
                .FirstOrDefaultAsync();
            

            return user?.Uno ?? 0;
        }

        public async Task<List<string>> GetUserLocationTypesAsync(int uno)
        {
            var locationTypes = await _dbEcheckContext.Ncumaps
                .Where(m => m.Uno == uno)
                .Join(
                    _dbEcheckContext.Ncmlocs,
                    map => new { map.Lcode, map.Oid },
                    loc => new { loc.Lcode, loc.Oid },
                    (map, loc) => loc.Ltype
                )
                 .Where(ltype => !string.IsNullOrWhiteSpace(ltype))
                 .Select(ltype => ltype.Trim())
                 .ToListAsync();

            return locationTypes;
        }



    }
}


