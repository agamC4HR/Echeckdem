using Echeckdem.Models;

namespace Echeckdem.Services
{
    public interface IUserService
    {
        Task<Users> AuthenticateUserAsync(string userID, string password);
        Task<int> GetUserLevelAsync(string userID);

    }
}
