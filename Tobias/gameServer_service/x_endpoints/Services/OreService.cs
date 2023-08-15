using MongoDB.Driver;
using x_endpoints.Models;
using System.Collections.Generic;
using x_endpoints.Persistence.MongoDB;

namespace x_endpoints.Services
{
    public class OreService : DataService<Ore>
    {
        public OreService(MongoDbService dbService) : base(dbService, "Ores") { }

         private readonly IMongoCollection<Ore> _ores;
    }
}