using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using x_endpoints.DomainModels._Interfaces;
using x_endpoints.DomainModels.Items;
using x_endpoints.Persistence.MongoDB;
using x_endpoints.Persistence.ServiceRegistration;

namespace x_endpoints.DomainServices
{
    public class WoodService : BaseService<Wood>
    {
        public WoodService(MongoDbService dbService) : base(dbService, "Wood")
        {
        }
    }
}