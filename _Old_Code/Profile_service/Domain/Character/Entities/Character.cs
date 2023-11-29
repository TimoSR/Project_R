using Domain.Character.Entities._Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Character.Entities;

public class Character
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string UserId { get; set; }
    public CharacterStats CharacterStats { get; set; }
    public IInventory Inventory { get; set; }
    public IEquipment Equipment { get; set; }
}