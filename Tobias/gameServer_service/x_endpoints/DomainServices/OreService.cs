using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using x_endpoints.DomainModels._Interfaces;
using x_endpoints.DomainModels.Equipments;
using x_endpoints.DomainModels.Items;
using x_endpoints.Persistence.MongoDB;
using x_endpoints.Persistence.ServiceRegistration;

namespace x_endpoints.DomainServices
{
    public class OreService : BaseService<Ore>
    {
        public OreService(MongoDbService dbService) : base(dbService, "Ores")
        {
           
        }
        public Dictionary<int,Ore> MineOre(Pickaxe pickaxe, Ore ore, int timeToMine)
        {
            Dictionary<int, Ore> dict = new Dictionary<int, Ore>();
            var oresMined = 0;
            if (ore.Requirement.LevelRequirement <= pickaxe.LevelRequirement)
            {
                Thread.Sleep(timeToMine);
                oresMined = (timeToMine / ore.Duration);
                
            }
            else
            {
                Console.WriteLine("You need a " + pickaxe.Name + " or better to mine this ore");

                oresMined = 0;
            }
            dict.Add(oresMined,ore);
            return dict;
        }
    }
}