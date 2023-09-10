using x_endpoints.DomainModels;
using x_endpoints.Persistence._Interfaces;

namespace x_endpoints.DomainRepositories.MongoDB;

public class ProductRepository : MongoRepository<Product>
{
    protected override string CollectionName => "Products";

    public ProductRepository(IMongoDbManager dbManager) : base(dbManager)
    {
    }
}