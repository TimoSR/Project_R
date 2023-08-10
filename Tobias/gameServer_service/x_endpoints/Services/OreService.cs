using MongoDB.Driver;
using x_endpoints.Models;
using System.Collections.Generic;
using x_endpoints.Persistence.MongoDB;

namespace x_endpoints.Services
{
    public class OreService
    {
        private readonly IMongoCollection<Ore> _ores;


        
        public OreService(MongoDbService dbService)
        {
            _ores = dbService.GetDefaultDatabase().GetCollection<Ore>("Ores");
        }


        public async Task InsertProduct(Ore ore)
        {
            await _ores.InsertOneAsync(ore);

            //var topicID = _pubServices.GenerateTopicID("SERVICE_NAME", "TOPIC_PRODUCT_UPDATES");
            //Console.WriteLine(topicID);

            // Publish a message after inserting a product.
            //await _pubServices.PublishMessageAsync(topicID, $"New product: {product.Name}");
        }
        public async Task<List<Ore>> GetAsync()
        {
            var ores = await _ores.Find(ore => true).ToListAsync();
            return ores;
        }
       

        public async Task<Ore> GetOreByIdAsync(string id)
        {
            var filter = Builders<Ore>.Filter.Eq(ore => ore.Id, id);
            var ore = await _ores.Find(filter).FirstOrDefaultAsync();
            return ore;
        }



       
        public async Task UpdateOreAsync(string id, Ore updatedOre)
        {
            var filter = Builders<Ore>.Filter.Eq(ore => ore.Id, id);

            var update = Builders<Ore>.Update
                .Set(ore => ore.Type, updatedOre.Type)
                .Set(ore => ore.Description, updatedOre.Description)
                .Set(ore => ore.Hits, updatedOre.Hits)
                .Set(ore => ore.Requiment, updatedOre.Requiment)
                .Set(ore => ore.Price, updatedOre.Price);

            await _ores.UpdateOneAsync(filter, update);
        }
        public async Task<bool> DeleteOreAsync(string id)
        {
            var filter = Builders<Ore>.Filter.Eq(ore => ore.Id, id);
            var result = await _ores.DeleteOneAsync(filter);

            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}