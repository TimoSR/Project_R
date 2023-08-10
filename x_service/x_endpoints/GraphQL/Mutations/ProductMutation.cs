using x_endpoints.Models;
using x_endpoints.Services;

namespace x_endpoints.GraphQL.Mutations;

public class ProductMutation: BaseMutation
{
    public async Task<Product> AddProduct(string name, string description, decimal price, [Service] ProductService productService)
    {
        var product = new Product { Name = name, Description = description, Price = price };
        await productService.InsertProduct(product);
        return product;
    }

    // Add more consolidated mutation methods...
}