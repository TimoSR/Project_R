using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using x_endpoints.DomainModels._Interfaces;
using x_endpoints.Enums;

namespace x_endpoints.DomainModels.Items;

public class Wood : IItems
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    [BsonRepresentation(BsonType.Int32)]
    public ItemRarity Rarity { get; set; }

    [BsonElement]
    public string Name { get; set; }

    [BsonElement]
    public string Description { get; set; }
    

    [BsonElement]
    public string Requirement {get; set;}

    [BsonElement]
    public decimal Price { get; set; }

    // You can add more properties depending on your needs. Each property represents a field in your Product document.
}