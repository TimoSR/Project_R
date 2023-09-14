using ServiceLibrary.Persistence._Interfaces;
using ServiceLibrary.Persistence.MongoDB;
using x_Domain.DomainModels;

namespace x_App.DomainRepositories;

public class ProductRepository : MongoRepository<Product>
{
    protected override string CollectionName => "Products";

    public ProductRepository(IMongoDbManager dbManager) : base(dbManager)
    {
    }
}