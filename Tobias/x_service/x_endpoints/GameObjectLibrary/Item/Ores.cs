using x_endpoints.DomainModels.Items;
using x_endpoints.DomainServices;
using x_endpoints.Enums;

namespace x_endpoints.GameObjectLibrary.Item
{
    public static class Ores
    {
        public static async Task SeedData(IServiceProvider serviceProvider)
        {
            var itemService = serviceProvider.GetRequiredService<OreService>();

            var ores = new List<Ore> 
            {
                new Ore
                {
                    Name = "Gold",
                    Description = "A precious metal",
<<<<<<< HEAD
                    Time = 100,
=======
                    Hits = "100",
>>>>>>> develop
                    Requirement = "Mining pick",
                    Price = 500,
                    Rarity = ItemRarity.Rare // Assign the rarity using the enum
                },
                new Ore
                {
                    Name = "Iron",
                    Description = "A common metal",
<<<<<<< HEAD
                    Time = 75,
=======
                    Hits = "75",
>>>>>>> develop
                    Requirement = "Mining pick",
                    Price = 100,
                    Rarity = ItemRarity.Common // Assign the rarity using the enum
                },
                new Ore
                {
                    Name = "Diamond",
                    Description = "A valuable gem",
<<<<<<< HEAD
                    Time = 150,
=======
                    Hits = "150",
>>>>>>> develop
                    Requirement = "Diamond pickaxe",
                    Price = 1000,
                    Rarity = ItemRarity.Epic // Assign the rarity using the enum
                },
                // Add more ore items as needed
            };
            
            foreach (var ore in ores)
            {
                await itemService.InsertAsync(ore);
            } 
            
        }                        
    }
}



