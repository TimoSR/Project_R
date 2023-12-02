using x_endpoints.DomainModels;
using x_endpoints.DomainModels.Items;
using x_endpoints.Persistence.MongoDB;
using x_endpoints.Persistence.ServiceRegistration;

namespace x_endpoints.DomainServices;

public class HideService : BaseService<Hide>
{
    public HideService(MongoDbService dbService) : base(dbService, "Hides")
    {
    }

       
}