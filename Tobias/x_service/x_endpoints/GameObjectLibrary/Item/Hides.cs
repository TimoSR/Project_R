using x_endpoints.DomainModels.Items;
using x_endpoints.DomainServices;
using x_endpoints.Enums;

namespace x_endpoints.GameObjectLibrary.Item
{
    public static class Hides
    {
        public static async Task SeedData(IServiceProvider serviceProvider)
        {
            var itemService = serviceProvider.GetRequiredService<HideService>();

            var hides = new List<Hide>
            {
                new Hide
                {
                    Name = "Wolf Hide",
                    Description = "The hide of a fierce wolf",
                    Requirement = "Skinning knife",
                    Price = 50,
                    Rarity = ItemRarity.Common // Assign the rarity using the enum
                },
                new Hide
                {
                    Name = "Bear Pelt",
                    Description = "A thick bear pelt",
                    Requirement = "Skinning knife",
                    Price = 80,
                    Rarity = ItemRarity.Uncommon // Assign the rarity using the enum
                },
                new Hide
                {
                    Name = "Lion Mane",
                    Description = "A majestic lion's mane",
                    Requirement = "Skinning knife",
                    Price = 100,
                    Rarity = ItemRarity.Rare // Assign the rarity using the enum
                },
                // Add more hide items as needed
            };

            foreach (var hide in hides)
            {
                await itemService.InsertAsync(hide);
            }
        }
    }
}