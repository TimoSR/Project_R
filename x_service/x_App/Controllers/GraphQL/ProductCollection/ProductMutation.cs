using x_App.Controllers.GraphQL._Interface;
using x_App.DomainAppServices;
using x_lib.DomainModels;

namespace x_App.Controllers.GraphQL.ProductCollection;

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