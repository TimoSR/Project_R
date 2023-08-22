using x_endpoints.DomainModels.Equipments;
using x_endpoints.Persistence.MongoDB;
using x_endpoints.Persistence.ServiceRegistration;

namespace x_endpoints.DomainServices;

public class WeaponService : BaseService<Weapon>
{
    public WeaponService(MongoDbService dbService) : base(dbService, "Weapons")
    {
    }
}