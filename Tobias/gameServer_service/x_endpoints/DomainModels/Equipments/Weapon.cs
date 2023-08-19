using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using x_endpoints.Enums;
using x_endpoints.Interfaces;

namespace x_endpoints.Models;

public class Weapon
{
    public class Weapon : IEquipment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; init; }

        [BsonElement]
        public string Name { get; set; }
        [BsonElement]
        public int LevelRequirement { get; set; }
        private EquipmentSlot _slot;
        
        [BsonElement]
        public EquipmentSlot Slot
        {
            get { return _slot; }
            set { _slot = EquipmentSlot.Weapon; } // Always set to Weapon
        }
        [BsonElement]
        public int AttackValue { get; set; }
    }
}  