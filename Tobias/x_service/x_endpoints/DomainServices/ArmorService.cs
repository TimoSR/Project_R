using x_endpoints.DomainModels.Equipments;
using x_endpoints.Persistence.MongoDB;
using x_endpoints.Persistence.ServiceRegistration;

namespace x_endpoints.DomainServices;

public class ArmorService : BaseService<Armor>
{
    public ArmorService(MongoDbService dbService) : base(dbService, "Armors")
    {
    }
}