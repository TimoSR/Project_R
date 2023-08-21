using x_endpoints.DomainModels.Equipments;
using x_endpoints.DomainServices;
using x_endpoints.Enums;

namespace x_endpoints.GameObjectLibrary.Equipment
{
    public static class Sword
    {
        public static async Task SeedData(IServiceProvider serviceProvider)
        {
            var itemService = serviceProvider.GetRequiredService<WeaponService>();

            var swords = new List<Weapon>
            {
                new Weapon
                {
                    Name = "Short Sword",
                    LevelRequirement = 10,
                    AttackValue = 20,
                    Rarity = ItemRarity.Common // Assign the rarity using the enum
                },
                new Weapon
                {
                    Name = "Steel Dagger",
                    LevelRequirement = 15,
                    AttackValue = 18,
                    Rarity = ItemRarity.Uncommon // Assign the rarity using the enum
                },
                new Weapon
                {
                    Name = "Rapier",
                    LevelRequirement = 20,
                    AttackValue = 25,
                    Rarity = ItemRarity.Rare // Assign the rarity using the enum
                },
                new Weapon
                {
                    Name = "Curved Blade",
                    LevelRequirement = 25,
                    AttackValue = 22,
                    Rarity = ItemRarity.Common // Example, adjust as needed
                },
                new Weapon
                {
                    Name = "Enchanted Saber",
                    LevelRequirement = 30,
                    AttackValue = 30,
                    Rarity = ItemRarity.Epic // Assign the rarity using the enum
                }
            };

            foreach (var sword in swords)
            {
                await itemService.InsertAsync(sword);
            }
        }
    }
}



