using Echeckdem.Models;

namespace Echeckdem.Services
{
    public interface IUserService
    {
        bool IsValidUser(string userId, string password);
        bool IsValidUserhash(string userId, string password);
        Task<int> GetUserLevelAsync(string Userid);
        Task<int> GetUserUnoAsync(string Userid);

        Task<List<string>> GetUserLocationTypesAsync(int uno);


    }
}
