using x_endpoints.DomainAppServices;
using x_endpoints.Registration.GraphQL.Queries;

namespace x_endpoints.ControllersGraphQL.ProductCollection;

public class ProductQuery : BaseQuery
{
    private readonly ProductAppService _productAppService;
    
    public ProductQuery(ProductAppService productAppService)
    {
        _productAppService = productAppService;
    }
    
    public async Task<IEnumerable<x_lib.DomainModels.Product>> GetProducts() 
    {
        return await _productAppService.GetAllAsync();
    }
}