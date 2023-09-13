using x_endpoints.Infrastructure.Persistence._Interfaces;
using x_endpoints.Infrastructure.Persistence.MongoDB;
using x_lib.DomainModels;

namespace x_endpoints.DomainRepositories.MongoDB;

public class ProductRepository : MongoRepository<Product>
{
    protected override string CollectionName => "Products";

    public ProductRepository(IMongoDbManager dbManager) : base(dbManager)
    {
    }
}