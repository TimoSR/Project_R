using MongoDB.Bson;
using x_endpoints.Enums;
using x_endpoints.Interfaces;
using x_endpoints.Models;

namespace x_endpoints.GameObjectLibrary.Equipment.Weapon.OneHander;

public class Sword
{
    public List<Weapon> oneHandSwords = new List<Weapon>
    {
             new Weapon
             {
                 Id = ObjectId.GenerateNewId().ToString(),
                 Name = "Short Sword",
                 LevelRequirement = 10,
                 Slot = EquipmentSlot.Weapon,
                 AttackValue = 20
             },
             new Weapon
             {
                 Id = ObjectId.GenerateNewId().ToString(),
                 Name = "Steel Dagger",
                 LevelRequirement = 15,
                 Slot = EquipmentSlot.Weapon,
                 AttackValue = 18
             },
             new Weapon
             {
                 Id = ObjectId.GenerateNewId().ToString(),
                 Name = "Rapier",
                 LevelRequirement = 20,
                 Slot = EquipmentSlot.Weapon,
                 AttackValue = 25
             },
             new Weapon
             {
                 Id = ObjectId.GenerateNewId().ToString(),
                 Name = "Curved Blade",
                 LevelRequirement = 25,
                 Slot = EquipmentSlot.Weapon,
                 AttackValue = 22
             },
             new Weapon
             {
                 Id = ObjectId.GenerateNewId().ToString(),
                 Name = "Enchanted Saber",
                 LevelRequirement = 30,
                 Slot = EquipmentSlot.Weapon,
                 AttackValue = 30
             }
             // Add more one-handed swords as needed
         };
    
}