using x_endpoints.DomainModels.Equipments;
using x_endpoints.DomainModels.Items;
using x_endpoints.DomainServices;
using x_endpoints.Enums;
using x_endpoints.GameObjectLibrary.Equipment;
using x_endpoints.Persistence.DataSeeder;

namespace x_endpoints.GameObjectLibrary.Item
{
    public class Ores : IDataSeeder
    {
        public async Task SeedData(IServiceProvider serviceProvider)
        {
            
            var itemService = serviceProvider.GetRequiredService<OreService>();
            List<Pickaxe> pickaxes = Pickaxes._pickaxes;
            var ores = new List<Ore> 
            {
                new Ore
                {
                    Name = "Gold",
                    Description = "A precious metal",
                    Duration = 100,
                    Requirement = pickaxes[1],
                    Price = 500,
                    Rarity = ItemRarity.Rare // Assign the rarity using the enum
                },
                new Ore
                {
                    Name = "Iron",
                    Description = "A common metal",
                    Duration = 75,
                    Requirement = pickaxes[0],
                    Price = 100,
                    Rarity = ItemRarity.Common // Assign the rarity using the enum
                },
                new Ore
                {
                    Name = "Diamond",
                    Description = "A valuable gem",
                    Duration = 150,
                    Requirement = pickaxes[2],
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



