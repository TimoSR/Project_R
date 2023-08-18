using MongoDB.Bson;
using x_endpoints.DomainServices;
using x_endpoints.Models;

namespace x_endpoints.Seeders;

public static class DataSeeder
{
    public static async void SeedData(IServiceProvider serviceProvider)
    {
        var oreService =serviceProvider.GetRequiredService<OreService>();
        var productService = serviceProvider.GetRequiredService<ProductService>();

        Ore ore1 = new Ore
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Type = "Iron",
            Description = "High-quality iron ore",
            Hits = "Strong",
            Requiment = "Pickaxe",
            Price = 15.99M
        };

        Ore ore2 = new Ore
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Type = "Gold",
            Description = "Rare and valuable gold ore",
            Hits = "Moderate",
            Requiment = "Gold pickaxe",
            Price = 129.99M
        };

        Ore ore3 = new Ore
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Type = "Diamond",
            Description = "Precious diamond ore",
            Hits = "Very strong",
            Requiment = "Diamond pickaxe",
            Price = 299.99M
        };
        
        await oreService.InsertAsync(ore1);
        await oreService.InsertAsync(ore2);
        await oreService.InsertAsync(ore3);
    }
}