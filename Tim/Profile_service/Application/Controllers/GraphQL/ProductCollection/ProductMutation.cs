using Application.AppServices;
using Application.AppServices.V1._Interfaces;
using Application.Registrations._Interfaces;
using Domain.Common.DomainModels;
using Domain.DomainModels;

namespace Application.Controllers.GraphQL.ProductCollection;

public class ProductMutation : IMutation
{
    private readonly IProductService _productService;
    
    public ProductMutation(IProductService productService)
    {
        _productService = productService;
    }
    
    public async Task<Product> AddProduct(string name, string description, double price)
    {
        var product = new Product { Name = name, Description = description, Price = price };
        await _productService.InsertAsync(product);
        return product;
    }
}