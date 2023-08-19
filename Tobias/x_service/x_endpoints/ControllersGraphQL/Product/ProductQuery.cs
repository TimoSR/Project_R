using x_endpoints.DomainServices;
using x_endpoints.Persistence.GraphQL_Server.Queries;

namespace x_endpoints.ControllersGraphQL.Product;

public class ProductQuery : BaseQuery
{
    public async Task<List<DomainModels.Product>> GetProducts([Service] ProductService productService) 
    {
        return await productService.GetAllAsync();
    }

    // Add more consolidated query methods...
}