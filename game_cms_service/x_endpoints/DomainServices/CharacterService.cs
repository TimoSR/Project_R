using MongoDB.Driver;
using x_endpoints.DomainModels;
using x_endpoints.Persistence.MongoDB;
using x_endpoints.Persistence.ServiceRegistration;

namespace x_endpoints.DomainServices
{
    public class CharacterService : BaseService<Character>
    {
        public CharacterService(MongoDbService dbService) : base(dbService, "Characters")
        {
        }

       
    }
}