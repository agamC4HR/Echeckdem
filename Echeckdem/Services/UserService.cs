using Echeckdem.MongoData;
using Echeckdem.Models;
using MongoDB.Driver;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace Echeckdem.Services
{
    public class UserService: IUserService
    {
        private readonly MongoDbService _mongoDbService;

        public UserService(MongoDbService mongoDbService)

        {
            _mongoDbService = mongoDbService;
        }

        public async Task<Users> AuthenticateUserAsync(string userID, string password)
            {

            var usersCollection = _mongoDbService.GetUsersCollection();
            var filter = Builders<Users>.Filter.And(
                Builders<Users>.Filter.Eq(u => u.userID, userID),
                Builders<Users>.Filter.Eq(u => u.password, password)
                );

            return await usersCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<int> GetUserLevelAsync(string userID)              //getting USERLEVEL
        {
            var usersCollection = _mongoDbService.GetUsersCollection();
            var filter = Builders<Users>.Filter.Eq(u => u.userID, userID);
            var user = await usersCollection.Find(filter).FirstOrDefaultAsync();

            return user?.Ulev ?? 0; // Assuming `ulev` is the field for user level  
        }

    }
}

