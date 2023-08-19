using MongoDB.Bson;
using x_endpoints.Models;

namespace x_endpoints.Seeders.Items.Bars;

public class Bars
{
    
            public List<Bar> metalBars = new List<Bar>
            {
                new Bar
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "Golden Metal Bar",
                    Description = "A bar made of pure gold",
                    Type = "Metal",
                    Price = 1500,
                    Capacity = 100,
                    Rating = 4.5
                },
                new Bar
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "Ironworks Bar",
                    Description = "A sturdy bar made from iron",
                    Type = "Metal",
                    Price = 300,
                    Capacity = 50,
                    Rating = 4.0
                },
                new Bar
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "Diamond-Encrusted Bar",
                    Description = "A lavish bar with diamond accents",
                    Type = "Metal",
                    Price = 2500,
                    Capacity = 75,
                    Rating = 4.8
                },
                new Bar
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "Copper Craftsmen Bar",
                    Description = "A bar showcasing intricate copper designs",
                    Type = "Metal",
                    Price = 200,
                    Capacity = 60,
                    Rating = 3.7
                },
                new Bar
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "Emerald-Embellished Bar",
                    Description = "A metal bar adorned with emerald elements",
                    Type = "Metal",
                    Price = 800,
                    Capacity = 40,
                    Rating = 4.2
                },
                new Bar
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "Coal Forge Bar",
                    Description = "A bar inspired by coal and metalworking",
                    Type = "Metal",
                    Price = 150,
                    Capacity = 30,
                    Rating = 3.5
                },
                new Bar
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "Ruby-Inlaid Bar",
                    Description = "A metal bar with ruby accents",
                    Type = "Metal",
                    Price = 1800,
                    Capacity = 80,
                    Rating = 4.7
                }
                // Add more metal bars as needed
            };
}