using x_endpoints.DomainAppServices;
using x_endpoints.Infrastructure.Registration.GraphQL.Queries;
using x_lib.DomainModels;

namespace x_endpoints.ControllersGraphQL.ProductCollection;

public class ProductQuery : BaseQuery
{
    private readonly ProductAppService _productAppService;
    
    public ProductQuery(ProductAppService productAppService)
    {
        _productAppService = productAppService;
    }
    
    public async Task<IEnumerable<Product>> GetProducts() 
    {
        return await _productAppService.GetAllAsync();
    }
}