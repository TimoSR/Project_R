using x_endpoints.DomainModels.Equipments;
using x_endpoints.DomainServices;
using x_endpoints.Enums;

namespace x_endpoints.GameObjectLibrary.Equipment
{
    public static class Head
    {
        public static async Task SeedData(IServiceProvider serviceProvider)
        {
            var armorService = serviceProvider.GetRequiredService<ArmorService>();

            var heads = new List<Armor>
            {
                new Armor
                {
                    Name = "Iron Helmet",
                    LevelRequirement = 10,
                    ArmorValue = 12,
                    Slot = EquipmentSlot.Head,
                    Rarity = ItemRarity.Common // Assign the rarity using the enum
                },
                new Armor
                {
                    Name = "Steel Helm",
                    LevelRequirement = 15,
                    ArmorValue = 18,
                    Slot = EquipmentSlot.Head,
                    Rarity = ItemRarity.Uncommon // Assign the rarity using the enum
                },
                new Armor
                {
                    Name = "Enchanted Crown",
                    LevelRequirement = 20,
                    ArmorValue = 25,
                    Slot = EquipmentSlot.Head,
                    Rarity = ItemRarity.Rare // Assign the rarity using the enum
                },
                // Add more head armor items as needed
            };

            foreach (var head in heads)
            {
                await armorService.InsertAsync(head);
            }
        }
    }
}
