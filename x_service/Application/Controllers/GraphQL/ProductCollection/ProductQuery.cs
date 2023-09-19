using Application.AppServices;
using Application.Registrations._Interfaces;
using Domain.DomainModels;

namespace Application.Controllers.GraphQL.ProductCollection;

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