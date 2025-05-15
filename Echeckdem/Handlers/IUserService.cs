using Echeckdem.Models;

namespace Echeckdem.Handlers
{
    public interface IUserService
    {
        bool IsValidUser(string userId, string password);
        bool IsValidUserhash(string userId, string password);
        Task<int> GetUserLevelAsync(string Userid);
        Task<int> GetUserUnoAsync(string Userid);

        Task<List<string>> GetUserLocationTypesAsync(int uno);

        Task<List<Ncumap>> GetUserLocationsAsync(int uno);
    }
}
