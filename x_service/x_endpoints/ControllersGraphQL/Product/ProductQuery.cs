using x_endpoints.DomainServices;
using x_endpoints.Registration.GraphQL.Queries;

namespace x_endpoints.ControllersGraphQL.Product;

public class ProductQuery : BaseQuery
{
    public async Task<IEnumerable<DomainModels.Product>> GetProducts([Service] ProductService productService) 
    {
        return await productService.GetAllAsync();
    }

    // Add more consolidated query methods...
}