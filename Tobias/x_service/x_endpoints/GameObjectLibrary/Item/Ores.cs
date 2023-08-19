using x_endpoints.DomainModels.Items;
using x_endpoints.DomainServices;

namespace x_endpoints.GameObjectLibrary.Item;

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
                Hits = "100",
                Requirement = "Mining pick",
                Price = 500
            },
            new Ore
            {
                Name = "Iron",
                Description = "A common metal",
                Hits = "75",
                Requirement = "Mining pick",
                Price = 100
            },
            new Ore
            {
                Name = "Diamond",
                Description = "A valuable gem",
                Hits = "150",
                Requirement = "Diamond pickaxe",
                Price = 1000
            },
            new Ore
            {
                Name = "Copper",
                Description = "A versatile metal",
                Hits = "90",
                Requirement = "Mining pick",
                Price = 150
            },
            new Ore
            {
                Name = "Emerald",
                Description = "A precious green gem",
                Hits = "130",
                Requirement = "Iron pickaxe",
                Price = 800
            },
            new Ore
            {
                Name = "Coal",
                Description = "A fossil fuel",
                Hits = "60",
                Requirement = "Wooden pickaxe",
                Price = 50
            },
            new Ore
            {
                Name = "Ruby",
                Description = "A deep red gem",
                Hits = "140",
                Requirement = "Diamond pickaxe",
                Price = 1200
            }
        };
        
        foreach (var ore in ores)
        {
            await itemService.InsertAsync(ore);
        } 
        
    }                        
}


