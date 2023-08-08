using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace x_endpoints.Models;

public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("Name")]
    public string Name { get; set; }

    [BsonElement("Description")]
    public string Description { get; set; }

    [BsonElement("Price")]
    public decimal Price { get; set; }

    // You can add more properties depending on your needs. Each property represents a field in your Product document.
}