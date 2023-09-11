using x_endpoints.DomainAppServices;
using x_endpoints.Registration.GraphQL.Mutations;
using x_lib.DomainModels;

namespace x_endpoints.ControllersGraphQL.Products;

public class ProductMutation : BaseMutation
{
    public async Task<x_lib.DomainModels.Product> AddProduct(string name, string description, double price, [Service] ProductAppService productAppService)
    {
        var product = new x_lib.DomainModels.Product { Name = name, Description = description, Price = price };
        await productAppService.InsertAsync(product);
        return product;
    }

    // Add more consolidated mutation methods...
}