using x_App.DomainAppServices;
using x_App.Infrastructure.Reflectors._Interfaces;
using x_Domain.DomainModels;

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