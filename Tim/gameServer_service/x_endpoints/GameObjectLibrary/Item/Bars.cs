using x_endpoints.DomainModels.Items;
using x_endpoints.DomainServices;

namespace x_endpoints.GameObjectLibrary.Item;

public static class Bars
{
    public static async Task SeedData(IServiceProvider serviceProvider)
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
                Rating = 4.5
            },
            new Bar
            {
                Name = "Ironworks Bar",
                Description = "A sturdy bar made from iron",
                Price = 300,
                Capacity = 50,
                Rating = 4.0
            },
            new Bar
            {
                Name = "Diamond-Encrusted Bar",
                Description = "A lavish bar with diamond accents",
                Price = 2500,
                Capacity = 75,
                Rating = 4.8
            },
            new Bar
            {
                Name = "Copper Craftsmen Bar",
                Description = "A bar showcasing intricate copper designs",
                Price = 200,
                Capacity = 60,
                Rating = 3.7
            },
            new Bar
            {
                Name = "Emerald-Embellished Bar",
                Description = "A metal bar adorned with emerald elements",
                Price = 800,
                Capacity = 40,
                Rating = 4.2
            },
            new Bar
            {
                Name = "Coal Forge Bar",
                Description = "A bar inspired by coal and metalworking",
                Price = 150,
                Capacity = 30,
                Rating = 3.5
            },
            new Bar
            {
                Name = "Ruby-Inlaid Bar",
                Description = "A metal bar with ruby accents",
                Price = 1800,
                Capacity = 80,
                Rating = 4.7
            }
        };
        
        foreach (var bar in bars)
        {
            await itemService.InsertAsync(bar);
        }
        
    }
}