using Application.AppServices;
using Application.AppServices.V1._Interfaces;
using Application.Registrations._Interfaces;
using Domain.DomainModels;

namespace Application.Controllers.GraphQL.ProductCollection;

public class ProductQuery : IQuery
{
    private readonly IProductService _productService;
    
    public ProductQuery(IProductService productService)
    {
        _productService = productService;
    }
    
    public async Task<IEnumerable<Product>> GetProducts() 
    {
        return await _productService.GetAllAsync();
    }
}