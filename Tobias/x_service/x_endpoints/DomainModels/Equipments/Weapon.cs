using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using x_endpoints.DomainModels._Interfaces;
using x_endpoints.Enums;

namespace x_endpoints.DomainModels.Equipments;

public class Weapon : IItems, IEquipment
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    [BsonElement]
    public string Name { get; set; }

    [BsonElement]
    public string Description { get; set; }
    
    [BsonElement]
    public decimal Price { get; set; }

    [BsonElement]
    public int LevelRequirement { get; set; }
    
    [BsonRepresentation(BsonType.Int32)]
    public EquipmentSlot Slot { get; } = EquipmentSlot.Weapon;
    
    [BsonElement]
    public int AttackValue { get; set; }
}


