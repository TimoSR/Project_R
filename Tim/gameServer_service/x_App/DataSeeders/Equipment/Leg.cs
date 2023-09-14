using x_endpoints.DomainModels.Equipments;
using x_endpoints.DomainServices;
using x_endpoints.Enums;
using x_endpoints.Persistence.DataSeeder;
using x_lib.DomainModels._Enums;

namespace x_endpoints.GameObjectLibrary.Equipment
{
    public class Leg : IDataSeeder
    {
        public async Task SeedData(IServiceProvider serviceProvider)
        {
            var armorService = serviceProvider.GetRequiredService<ArmorService>();

            var legs = new List<Armor>
            {
                new Armor
                {
                    Name = "Iron Leggings",
                    LevelRequirement = 10,
                    ArmorValue = 15,
                    Slot = EquipmentSlot.Legs,
                    Rarity = ItemRarity.Common // Assign the rarity using the enum
                },
                new Armor
                {
                    Name = "Steel Greaves",
                    LevelRequirement = 15,
                    ArmorValue = 20,
                    Slot = EquipmentSlot.Legs,
                    Rarity = ItemRarity.Uncommon // Assign the rarity using the enum
                },
                new Armor
                {
                    Name = "Enchanted Legplates",
                    LevelRequirement = 20,
                    ArmorValue = 25,
                    Slot = EquipmentSlot.Legs,
                    Rarity = ItemRarity.Rare // Assign the rarity using the enum
                },
                // Add more leg armor items as needed
            };

            foreach (var leg in legs)
            {
                await armorService.InsertAsync(leg);
            }
        }
    }
}
