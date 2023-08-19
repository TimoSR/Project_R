using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using x_endpoints.DomainModels._Interfaces;

namespace x_endpoints.DomainModels.Items;

public class Ore : IItems
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement]
    public string Name { get; set; }

    [BsonElement]
    public string Description { get; set; }

    [BsonElement]
    public string Hits {get; set;}

    [BsonElement]
    public string Requirement {get; set;}

    [BsonElement]
    public decimal Price { get; set; }

    // You can add more properties depending on your needs. Each property represents a field in your Product document.
}