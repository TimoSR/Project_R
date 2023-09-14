using x_App.Infrastructure.Persistence._Interfaces;
using x_App.Infrastructure.Persistence.MongoDB;
using x_lib.DomainModels;

namespace x_App.DomainRepositories.MongoDB;

public class ProductRepository : MongoRepository<Product>
{
    protected override string CollectionName => "Products";

    public ProductRepository(IMongoDbManager dbManager) : base(dbManager)
    {
    }
}