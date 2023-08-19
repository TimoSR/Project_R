using MongoDB.Bson;
using x_endpoints.Models;

namespace x_endpoints.Seeders.Items.Ores;

public class Ores
{            List<Ore> ores = new List<Ore>{
                    new Ore
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        Type = "Gold",
                        Description = "A precious metal",
                        Hits = "100",
                        Requiment = "Mining pick",
                        Price = 500
                    },
                    new Ore
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        Type = "Iron",
                        Description = "A common metal",
                        Hits = "75",
                        Requiment = "Mining pick",
                        Price = 100
                    },
                    new Ore
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        Type = "Diamond",
                        Description = "A valuable gem",
                        Hits = "150",
                        Requiment = "Diamond pickaxe",
                        Price = 1000
                    },
                    new Ore
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        Type = "Copper",
                        Description = "A versatile metal",
                        Hits = "90",
                        Requiment = "Mining pick",
                        Price = 150
                    },
                    new Ore
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        Type = "Emerald",
                        Description = "A precious green gem",
                        Hits = "130",
                        Requiment = "Iron pickaxe",
                        Price = 800
                    },
                    new Ore
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        Type = "Coal",
                        Description = "A fossil fuel",
                        Hits = "60",
                        Requiment = "Wooden pickaxe",
                        Price = 50
                    },
                    new Ore
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        Type = "Ruby",
                        Description = "A deep red gem",
                        Hits = "140",
                        Requiment = "Diamond pickaxe",
                        Price = 1200
                    }
                    // Add more ores as needed
                };
}