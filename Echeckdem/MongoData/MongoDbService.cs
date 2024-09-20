using MongoDB.Driver;
using Echeckdem.Models;
using Microsoft.IdentityModel.Tokens;

namespace Echeckdem.MongoData
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;

        public MongoDbService(IConfiguration configuration)
        {
            var connectionString = configuration["MongoDB:ConnectionString"];
            var database = configuration["MongoDB:Database"];

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "MongoDB connection string is not configured.");
            }

            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentNullException(nameof(database), "MongoDB database name is not configured.");
            }

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(database);
        }
        public IMongoCollection<Users> GetUsersCollection()
        {
            return _database.GetCollection<Users>("Users");
        }                
        
    }

              
}


