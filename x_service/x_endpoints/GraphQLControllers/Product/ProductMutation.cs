using x_endpoints.Persistence.GraphQL_Server.Mutations;
using x_endpoints.Services;

namespace x_endpoints.GraphQL.Product;

public class ProductMutation: BaseMutation
{
    public async Task<Models.Product> AddProduct(string name, string description, decimal price, [Service] ProductService productService)
    {
        var product = new Models.Product { Name = name, Description = description, Price = price };
        await productService.InsertProduct(product);
        return product;
    }

    // Add more consolidated mutation methods...
}