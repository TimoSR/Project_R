using x_endpoints.Models;
using x_endpoints.Services;

namespace x_endpoints.DataSeeder;

public static class DataSeeder
{
    public static void SeedData(IServiceProvider serviceProvider)
    {
        var productService = serviceProvider.GetRequiredService<ProductService>();

        var product1 = new Product
        {
            Name = "Product 1",
            Description = "This is product 1",
            Price = 29.99m
        };

        var product2 = new Product
        {
            Name = "Product 2",
            Description = "This is product 2",
            Price = 39.99m
        };

        productService.InsertProduct(product1);
        productService.InsertProduct(product2);
    }
}