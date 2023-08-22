using x_endpoints.DomainModels.Items;
using x_endpoints.DomainServices;
using x_endpoints.Enums;

namespace x_endpoints.GameObjectLibrary.Item
{
    public static class Woods
    {
        public static async Task SeedData(IServiceProvider serviceProvider)
        {
            var itemService = serviceProvider.GetRequiredService<WoodService>();

            var woods = new List<Wood>
            {
                new Wood
                {
                    Name = "Oak Wood",
                    Description = "Wood harvested from an oak tree",
                    Requirement = "Axe",
                    Price = 10,
                    Rarity = ItemRarity.Common // Assign the rarity using the enum
                },
                new Wood
                {
                    Name = "Pine Wood",
                    Description = "Wood obtained from a pine tree",
                    Requirement = "Axe",
                    Price = 8,
                    Rarity = ItemRarity.Common // Assign the rarity using the enum
                },
                new Wood
                {
                    Name = "Mahogany Wood",
                    Description = "Luxurious wood from a mahogany tree",
                    Requirement = "Golden Axe",
                    Price = 50,
                    Rarity = ItemRarity.Rare // Assign the rarity using the enum
                },
                // Add more wood items as needed
            };

            foreach (var wood in woods)
            {
                await itemService.InsertAsync(wood);
            }
        }
    }
}
