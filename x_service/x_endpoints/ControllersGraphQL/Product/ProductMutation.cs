using x_endpoints.DomainServices;
using x_endpoints.Persistence.GraphQL_Server.Mutations;

namespace x_endpoints.ControllersGraphQL.Product;

public class ProductMutation : BaseMutation
{
    public async Task<DomainModels.Product> AddProduct(string name, string description, decimal price, [Service] ProductService productService)
    {
        var product = new DomainModels.Product { Name = name, Description = description, Price = price };
        await productService.InsertAsync(product);
        return product;
    }

    // Add more consolidated mutation methods...
}