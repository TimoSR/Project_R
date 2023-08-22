using x_endpoints.DomainModels.Items;
using x_endpoints.DomainServices;
using x_endpoints.Enums;
using x_endpoints.Persistence.DataSeeder;

namespace x_endpoints.GameObjectLibrary.Item
{
    public class Ores : IDataSeeder
    {
        public async Task SeedData(IServiceProvider serviceProvider)
        {
            var itemService = serviceProvider.GetRequiredService<OreService>();

            var ores = new List<Ore> 
            {
                new Ore
                {
                    Name = "Gold",
                    Description = "A precious metal",
                    Hits = "100",
                    Requirement = "Mining pick",
                    Price = 500,
                    Rarity = ItemRarity.Rare // Assign the rarity using the enum
                },
                new Ore
                {
                    Name = "Iron",
                    Description = "A common metal",
                    Hits = "75",
                    Requirement = "Mining pick",
                    Price = 100,
                    Rarity = ItemRarity.Common // Assign the rarity using the enum
                },
                new Ore
                {
                    Name = "Diamond",
                    Description = "A valuable gem",
                    Hits = "150",
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



