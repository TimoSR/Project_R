using x_endpoints.ControllerServices;
using x_endpoints.Persistence.GraphQL_Server.Queries;

namespace x_endpoints.GraphQLControllers.Product;

public class ProductQuery : BaseQuery
{
    public async Task<List<Models.Product>> GetProducts([Service] ProductService productService) 
    {
        return await productService.GetAsync();
    }

    // Add more consolidated query methods...
}