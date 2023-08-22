using x_endpoints.DomainModels.Items;
using x_endpoints.DomainServices;
using x_endpoints.Enums;
using x_endpoints.Persistence.DataSeeder;

namespace x_endpoints.GameObjectLibrary.Item
{
    public class Leathers : IDataSeeder
    {
        public async Task SeedData(IServiceProvider serviceProvider)
        {
            var itemService = serviceProvider.GetRequiredService<LeatherSerivce>();

            var leathers = new List<Leather>
            {
                new Leather
                {
                    Name = "Wolf Leather",
                    Description = "Leather crafted from a wolf's hide",
                    Requirement = "Tanning solution",
                    Price = 80,
                    Rarity = ItemRarity.Uncommon // Assign the rarity using the enum
                },
                new Leather
                {
                    Name = "Bear Leather",
                    Description = "Thick leather made from a bear's pelt",
                    Requirement = "Tanning solution",
                    Price = 120,
                    Rarity = ItemRarity.Rare // Assign the rarity using the enum
                },
                new Leather
                {
                    Name = "Lion Leather",
                    Description = "Supple leather from a lion's hide",
                    Requirement = "Tanning solution",
                    Price = 150,
                    Rarity = ItemRarity.Epic // Assign the rarity using the enum
                },
                // Add more leather items as needed
            };

            foreach (var leather in leathers)
            {
                await itemService.InsertAsync(leather);
            }
        }
    }
}