using Application.AppServices;
using Application.Registrations._Interfaces;
using Domain.DomainModels;

namespace Application.Controllers.GraphQL.ProductCollection;

public class ProductMutation : IMutation
{
    private readonly ProductAppService _productAppService;
    
    public ProductMutation(ProductAppService productAppService)
    {
        _productAppService = productAppService;
    }
    
    public async Task<Product> AddProduct(string name, string description, double price)
    {
        var product = new Product { Name = name, Description = description, Price = price };
        await _productAppService.InsertAsync(product);
        return product;
    }
}