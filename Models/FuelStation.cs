using MongoDB.Bson.Serialization.Attributes;
using System;
using MongoDB.Bson;

/*
* FuelStation: class - Represents fuel station model in database
*/
namespace equeue_server.Models
{
    [BsonIgnoreExtraElements]
    public class FuelStation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        [BsonElement("registrationNumber")]
        public String RegistrationNumber { get; set; } = String.Empty;

        [BsonElement("name")]
        public string Name { get; set; } = String.Empty;

        [BsonElement("location")]
        public string Location { get; set; } = String.Empty;

        [BsonElement("noPumps")]
        public String NoPumps { get; set; } = String.Empty;

        [BsonElement("availability")]
        public String Availability { get; set; } = String.Empty;

        [BsonElement("arrivalTime")]
        public String ArrivalTime { get; set; } = String.Empty;

        [BsonElement("finishTime")]
        public String FinishTime { get; set; } = String.Empty;

        public FuelStation() { }
    }
}
