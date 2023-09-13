using x_endpoints.DomainAppServices;
using x_endpoints.Infrastructure.Registration.GraphQL.Mutations;
using x_lib.DomainModels;

namespace x_endpoints.ControllersGraphQL.ProductCollection;

public class ProductMutation : BaseMutation
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