using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace x_endpoints.Models;

public class Character
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("Name")]
    public string Name { get; set; }

    [BsonElement("Health")]
    public string Health { get; set; }

    [BsonElement("Level")]
    public string Level {get; set;}
    
    


    // You can add more properties depending on your needs. Each property represents a field in your Product document.
}