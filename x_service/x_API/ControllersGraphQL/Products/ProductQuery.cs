using x_endpoints.DomainAppServices;
using x_endpoints.Registration.GraphQL.Queries;

namespace x_endpoints.ControllersGraphQL.Products;

public class ProductQuery : BaseQuery
{
    public async Task<IEnumerable<x_lib.DomainModels.Product>> GetProducts([Service] ProductAppService productAppService) 
    {
        return await productAppService.GetAllAsync();
    }

    // Add more consolidated query methods...
}