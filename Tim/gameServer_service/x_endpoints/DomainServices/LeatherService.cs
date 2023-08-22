using x_endpoints.DomainModels;
using x_endpoints.DomainModels.Items;
using x_endpoints.Persistence.MongoDB;
using x_endpoints.Persistence.ServiceRegistration;

namespace x_endpoints.DomainServices;

public class LeatherSerivce : BaseService<Leather>
{
    public LeatherSerivce(MongoDbService dbService) : base(dbService, "Leather")
    {
    }

       
}