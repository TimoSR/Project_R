// using x_endpoints.DomainModels;
// using x_endpoints.DomainServices;
// using x_endpoints.Registration.DataSeeder;
//
// namespace x_endpoints.Seeders;
//
// public class DataSeeder : IDataSeeder
// {
//     public async Task SeedData(IServiceProvider serviceProvider)
//     {
//         var productService = serviceProvider.GetRequiredService<ProductService>();
//
//         var products = new List<Product>
//         {
//             new Product
//             {
//                 Name = "Product 1",
//                 Description = "This is product 1",
//                 Price = 29.99m
//             },
//             new Product
//             {
//             Name = "Product 2",
//             Description = "This is product 2",
//             Price = 39.99m
//             }
//         };
//
//         foreach (var product in products)
//         {
//             await productService.InsertAsync(product);
//         }
//         
//     }
// }