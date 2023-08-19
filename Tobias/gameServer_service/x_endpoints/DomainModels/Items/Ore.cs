using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using x_endpoints.Interfaces;

namespace x_endpoints.Models;

public class Ore : IItems
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("Type")]
    public string Type { get; set; }

    [BsonElement("Description")]
    public string Description { get; set; }

    [BsonElement("Hits")]
    public string Hits {get; set;}

    [BsonElement("Requiment")]
    public string Requiment {get; set;}

    [BsonElement("Price")]
    public decimal Price { get; set; }

    // You can add more properties depending on your needs. Each property represents a field in your Product document.
}