using x_endpoints.DomainModels.Items;
using x_endpoints.Persistence.MongoDB;
using x_endpoints.Persistence.ServiceRegistration;

namespace x_endpoints.DomainServices;

public class BarServices : BaseService<Bar>
{
    public BarServices(MongoDbService dbService) : base(dbService, "Bars")
    {
    }
}