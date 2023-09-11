// using x_API.DomainModels;
// using x_API.DomainServices;
// using x_API.Registration.DataSeeder;
//
// namespace x_API.Seeders;
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