using x_App.Controllers.GraphQL._Interface;
using x_App.DomainAppServices;
using x_lib.DomainModels;

namespace x_App.Controllers.GraphQL.ProductCollection;

public class ProductQuery : IQuery
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