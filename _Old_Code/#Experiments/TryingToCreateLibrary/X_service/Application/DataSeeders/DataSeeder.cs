using Application.AppServices.V1;
using CommonLibrary.Application.Registrations.DataSeeder;
using Domain.DomainModels;

namespace Application.DataSeeders;

public class DataSeeder : IDataSeeder
{
    public async Task SeedData(IServiceProvider serviceProvider)
    {
        var productService = serviceProvider.GetRequiredService<ProductService>();

        var products = new List<Product>
        {
            new Product
            {
                Name = "Product 1",
                Description = "This is product 1",
                Price = 29.99
            },
            new Product
            {
            Name = "Product 2",
            Description = "This is product 2",
            Price = 39.99
            }
        };

        foreach (var product in products)
        {
            await productService.InsertAsync(product);
        }
        
    }
}