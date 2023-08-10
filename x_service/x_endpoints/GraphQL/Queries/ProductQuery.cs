using x_endpoints.Models;
using x_endpoints.Services;

namespace x_endpoints.GraphQL.Queries;

public class ProductQuery : BaseQuery
{
    public async Task<List<Product>> GetProducts([Service] ProductService productService) 
    {
        return await productService.GetAsync();
    }

    // Add more consolidated query methods...
}