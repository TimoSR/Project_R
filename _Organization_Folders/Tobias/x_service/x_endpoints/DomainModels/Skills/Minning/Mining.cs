using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using x_endpoints.DomainModels.Equipments;
using x_endpoints.DomainModels.Items;

namespace x_endpoints.DomainModels.Skills
{
    public class MiningSkill
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement]
        public string Name { get; set; }

        [BsonElement]
        public string Description { get; set; }

        [BsonElement]
        public int Level { get; set; }

        [BsonElement]
        public int Experience { get; set; }

        [BsonElement]
        public int KnowledgePoints { get; set; }

        // You can add more properties related to mining skill here.
        [BsonElement]
        public int MaxMiningDepth { get; set; }
        
        [BsonElement]
        public List<MiningRecipe> MiningRecipes { get; set; }

        // You can add methods or properties to interact with ores and mining veins.
        public int MineOre(Pickaxe pickaxe, Ore ore, int timeToMine, out Ore outOre)
        {
            outOre = new Ore();
            if (ore.Requirement.LevelRequirement <= pickaxe.LevelRequirement)
            {
                Thread.Sleep(timeToMine);
                outOre = ore;
                return timeToMine / ore.Time;
            }
            else
            {
                Console.WriteLine("You need a " + pickaxe.Name + " or better to mine this ore");
                return 0;
            }
        }
    }
}
