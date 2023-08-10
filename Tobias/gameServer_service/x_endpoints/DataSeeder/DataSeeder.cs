using MongoDB.Bson;
using x_endpoints.Models;
using x_endpoints.Services;

namespace x_endpoints.DataSeeder;

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
        var product1 = new Product
        {
            Name = "Product 1",
            Description = "This is product 1",
            Price = 29.99m
        };

        var product2 = new Product
        {
            Name = "Product 2",
            Description = "This is product 2",
            Price = 39.99m
        };
        
        await productService.InsertProduct(product1);
        await productService.InsertProduct(product2);
        await oreService.InsertProduct(ore1);
        await oreService.InsertProduct(ore2);
        await oreService.InsertProduct(ore3);
    }
}