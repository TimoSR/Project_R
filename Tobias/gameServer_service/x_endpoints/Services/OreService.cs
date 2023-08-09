using MongoDB.Driver;
using x_endpoints.Models;
using System.Collections.Generic;
using x_endpoints.Persistence.MongoDB;

namespace x_endpoints.Services
{
    public class _oreservice
    {
        private readonly IMongoCollection<Ore> _ores;


        
        public _oreservice(MongoDbService dbService)
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
        public List<Ore> Get() => _ores.Find(ore => true).ToList();
        // public List<Ore> Get_ores()
        // {
        //     return _oreCollection.Find(ore => true).ToList();
        // }

        // public Ore GetOreById(string id)
        // {
        //     return _oreCollection.Find(ore => ore.Id == id).FirstOrDefault();
        // }

        // public Ore CreateOre(Ore ore)
        // {
        //     _oreCollection.InsertOne(ore);
        //     return ore;
        // }

        // public void UpdateOre(string id, Ore updatedOre)
        // {
        //     _oreCollection.ReplaceOne(ore => ore.Id == id, updatedOre);
        // }

        // public void DeleteOre(string id)
        // {
        //     _oreCollection.DeleteOne(ore => ore.Id == id);
        // }
    }
}