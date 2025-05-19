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
using Echeckdem.Handlers;
using Echeckdem.ViewModel;

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
            if(user.Hashpassword==null) return false;

            var result = _passwordHasher.VerifyHashedPassword(user, user.Hashpassword, Password);
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

        public async Task<List<string>> GetUserLocationTypesAsync(int uno,int ulevel)
        {
            if (ulevel==1) 
            {
                var locationTypes = await (from a  in _dbEcheckContext.Ncmlocs
                                           join b in _dbEcheckContext.Ncmorgs on a.Oid equals b.Oid
                                           where a.Lactive == 1 && !string.IsNullOrEmpty(a.Ltype) && b.Oactive == 1
                select a.Ltype).Distinct().ToListAsync();


                return locationTypes;
            }
            else 
            {
                var locationTypes = await (
            from aa in _dbEcheckContext.Ncumaps.Where(x => x.Uno == uno)
            join bb in (
                from a in _dbEcheckContext.Ncmlocs
                where a.Lactive == 1 && !string.IsNullOrEmpty(a.Ltype)
                join b in _dbEcheckContext.Ncmorgs.Where(o => o.Oactive == 1) on a.Oid equals b.Oid
                select new { a.Oid, a.Lcode, a.Ltype }
            )
            on new { aa.Oid, aa.Lcode } equals new { bb.Oid, bb.Lcode }
            select bb.Ltype
        ).Distinct().ToListAsync();


                return locationTypes;
            }
                
        }

      
        public async Task<List<UserLocation>> GetUserLocationsAsync(int uno,int ulevel)
        {
            if (ulevel == 1)
            {
                return await (from client in _dbEcheckContext.Ncmorgs 
                              join locs in _dbEcheckContext.Ncmlocs on client.Oid equals locs.Oid
                              where client.Oactive == 1 && locs.Lactive == 1
                              orderby client.Oname, locs.Lname
                              select new UserLocation { Oid = client.Oid, Client = client.Oname, Lcode = locs.Lcode, Site = locs.Lname, Lcity = locs.Lcity, Lstate = locs.Lstate }).ToListAsync();
            }
            else {
                return await (from mapping in _dbEcheckContext.Ncumaps
                              join client in _dbEcheckContext.Ncmorgs on mapping.Oid equals client.Oid
                              join locs in _dbEcheckContext.Ncmlocs on mapping.Lcode equals locs.Lcode
                              where mapping.Uno == uno && client.Oactive == 1 && locs.Lactive == 1
                              orderby client.Oname, locs.Lname
                              select new UserLocation { Oid = client.Oid, Client = client.Oname, Lcode = locs.Lcode, Site = locs.Lname, Lcity = locs.Lcity, Lstate = locs.Lstate }).ToListAsync();
            }
                
            

        }
        public async Task<List<UserLocation>> GetUserBOLocationsAsync(int uno,int ulevel)
        {
            if (ulevel == 1)
            {
                return await (from client in _dbEcheckContext.Ncmorgs
                              join locs in _dbEcheckContext.Ncmlocs on client.Oid equals locs.Oid
                              where client.Oactive == 1 && locs.Lactive == 1 && !string.IsNullOrEmpty(locs.Ltype) && locs.Ltype.Trim() == "BO"
                              orderby client.Oname, locs.Lname
                              select new UserLocation { Oid = client.Oid, Client = client.Oname, Lcode = locs.Lcode, Site = locs.Lname, Lcity = locs.Lcity, Lstate = locs.Lstate }).ToListAsync();
            }
            else
            {
                return await (from mapping in _dbEcheckContext.Ncumaps
                              join client in _dbEcheckContext.Ncmorgs on mapping.Oid equals client.Oid
                              join locs in _dbEcheckContext.Ncmlocs on mapping.Lcode equals locs.Lcode
                              where mapping.Uno == uno && !string.IsNullOrEmpty(locs.Ltype) && locs.Ltype.Trim() == "BO" && client.Oactive == 1 && locs.Lactive == 1
                              orderby client.Oname, locs.Lname
                              select new UserLocation { Oid = client.Oid, Client = client.Oname, Lcode = locs.Lcode, Site = locs.Lname, Lcity = locs.Lcity, Lstate = locs.Lstate }).ToListAsync();
            }

        }

    }
}


