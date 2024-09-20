using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Echeckdem.Models
{
    public class Users
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]

        public string Id { get; set; }
        public string userID { get; set; }
        public string password { get; set; }
        public string oid { get; set; }
        public int Ulev {  get; set; }  

    }
}


