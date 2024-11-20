using Echeckdem.Models;
using MongoDB.Driver;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore;

namespace Echeckdem.Services
{
    public class UserService : IUserService
    {

        private readonly DbEcheckContext _dbEcheckContext;

        public UserService(DbEcheckContext dbEcheckContext)
        {
            _dbEcheckContext = dbEcheckContext;
        }

        public bool IsValidUser(string Userid, string Password)
        {
            var user = _dbEcheckContext.Ncusers.Where(u => u.Userid == Userid && u.Password == Password).FirstOrDefault();

            return user != null;
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


    }
}


