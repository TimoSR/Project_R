using MongoDB.Bson;
using x_endpoints.DomainServices;
using x_endpoints.Models;

namespace x_endpoints.Seeders;

public static class TestCharacters
{
    public static async void SeedData(IServiceProvider serviceProvider)
    {
        var oreService =serviceProvider.GetRequiredService<OreService>();
        var productService = serviceProvider.GetRequiredService<ProductService>();
        var characterService = serviceProvider.GetRequiredService<CharacterService>();

        List<Character> characters = new List<Character>
        {
            new Character
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Warrior",
                Health = "100",
                Level = "50"
            },
            new Character
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Mage",
                Health = "75",
                Level = "40"
            },
            new Character
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Rogue",
                Health = "80",
                Level = "45"
            }
        };
        
        foreach (var character in characters)
        {
            await characterService.InsertAsync(character);
        }
    }
}