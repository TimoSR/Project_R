using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace x_endpoints.DomainModels.Skills;

public class MiningRecipe
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement]
    public string Name { get; set; }

    [BsonElement]
    public string Description { get; set; }

    [BsonElement]
    public int LevelRequirement { get; set; }

}