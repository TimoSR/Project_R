using x_endpoints.DomainModels.Items;
using x_endpoints.DomainServices;
using x_endpoints.Enums;
using x_endpoints.Persistence.DataSeeder;

namespace x_endpoints.GameObjectLibrary.Item
{
    public class Bars : IDataSeeder
    {
        public async Task SeedData(IServiceProvider serviceProvider)
        {   
            var itemService = serviceProvider.GetRequiredService<BarServices>();
            
            var bars = new List<Bar>
            {
                new Bar
                {
                    Name = "Golden Metal Bar",
                    Description = "A bar made of pure gold",
                    Price = 1500,
                    Capacity = 100,
                    Rating = 4.5,
                    Rarity = ItemRarity.Epic // Assign the rarity using the enum
                },
                new Bar
                {
                    Name = "Ironworks Bar",
                    Description = "A sturdy bar made from iron",
                    Price = 300,
                    Capacity = 50,
                    Rating = 4.0,
                    Rarity = ItemRarity.Common // Assign the rarity using the enum
                },
                new Bar
                {
                    Name = "Diamond-Encrusted Bar",
                    Description = "A lavish bar with diamond accents",
                    Price = 2500,
                    Capacity = 75,
                    Rating = 4.8,
                    Rarity = ItemRarity.Legendary // Assign the rarity using the enum
                },
                // Add more bar items as needed
            };
            
            foreach (var bar in bars)
            {
                await itemService.InsertAsync(bar);
            }
            
        }
    }
}
