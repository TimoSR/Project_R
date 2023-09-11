using x_endpoints.DomainAppServices;
using x_endpoints.DomainServices;
using x_endpoints.Registration.GraphQL.Mutations;

namespace x_endpoints.ControllersGraphQL.Product;

public class ProductMutation : BaseMutation
{
    public async Task<x_lib.DomainModels.Product> AddProduct(string name, string description, decimal price, [Service] ProductAppService productAppService)
    {
        var product = new x_lib.DomainModels.Product { Name = name, Description = description, Price = price };
        await productAppService.InsertAsync(product);
        return product;
    }

    // Add more consolidated mutation methods...
}