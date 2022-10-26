/*
* User: class - user model in database is represented
*/

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace equeue_server.Models
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        [BsonElement("username")]
        public string Username { get; set; } = String.Empty;

        [BsonElement("password")]
        public string Password { get; set; } = String.Empty;

        [BsonElement("vehicleType")]
        public String VehicleType { get; set; } = String.Empty;

        [BsonElement("role")]
        public String Role { get; set; } = String.Empty;

        public User() {}
    }
}

