using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using x_endpoints.DomainModels._Interfaces;

namespace x_endpoints.DomainModels.Items
{
    public class Bar : IItems
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("Description")]
        public string Description {get; set;}

        [BsonElement("Price")]
        public decimal Price {get; set;}
        
        [BsonElement("Capacity")]
        public int Capacity { get; set; }

        [BsonElement("Rating")]
        public double Rating { get; set; }
    }
}