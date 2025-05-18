using Echeckdem.Models;
using Echeckdem.ViewModel;

namespace Echeckdem.Handlers
{
    public interface IUserService
    {
        bool IsValidUser(string userId, string password);
        bool IsValidUserhash(string userId, string password);
        Task<int> GetUserLevelAsync(string Userid);
        Task<int> GetUserUnoAsync(string Userid);

        Task<List<string>> GetUserLocationTypesAsync(int uno,int ulevel);

        //Task<List<Ncumap>> GetUserLocationsAsync(int uno);

        Task<List<UserLocation>> GetUserLocationsAsync(int uno, int ulevel);

        Task<List<UserLocation>> GetUserBOLocationsAsync(int uno, int ulevel);

    }
}
