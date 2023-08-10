using x_endpoints.ControllerServices;
using x_endpoints.Models;

namespace x_endpoints.DataSeeder;

public static class DataSeeder
{
    public static async void SeedData(IServiceProvider serviceProvider)
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

        await productService.InsertProduct(product1);
        await productService.InsertProduct(product2);
    }
}