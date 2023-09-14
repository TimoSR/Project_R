using x_endpoints.DomainModels.Equipments;
using x_endpoints.DomainServices;
using x_endpoints.Enums;
using x_endpoints.Persistence.DataSeeder;
using x_lib.DomainModels._Enums;

namespace x_endpoints.GameObjectLibrary.Equipment
{
        public class Chest : IDataSeeder
        {
            public async Task SeedData(IServiceProvider serviceProvider)
            {
                var armorService = serviceProvider.GetRequiredService<ArmorService>();

                var chests = new List<Armor>
                {
                    new Armor
                    {
                        Name = "Iron Chestplate",
                        LevelRequirement = 10,
                        ArmorValue = 20,
                        Slot = EquipmentSlot.Chest,
                        Rarity = ItemRarity.Common // Assign the rarity using the enum
                    },
                    new Armor
                    {
                        Name = "Steel Breastplate",
                        LevelRequirement = 15,
                        ArmorValue = 25,
                        Slot = EquipmentSlot.Chest,
                        Rarity = ItemRarity.Uncommon // Assign the rarity using the enum
                    },
                    new Armor
                    {
                        Name = "Enchanted Chestguard",
                        LevelRequirement = 20,
                        ArmorValue = 30,
                        Slot = EquipmentSlot.Chest,
                        Rarity = ItemRarity.Rare // Assign the rarity using the enum
                    },
                    // Add more chest armor items as needed
                };

                foreach (var chest in chests)
                {
                    await armorService.InsertAsync(chest);
                }
            }
        }
    

}