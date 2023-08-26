using Microsoft.AspNetCore.Mvc;
using x_endpoints.DomainModels;
using x_endpoints.DomainServices;

namespace x_endpoints.ControllersREST;

[ApiController]
[Route("api/[controller]")]
public class PubSubController : ControllerBase
{
    private readonly ProductService _productService;

    public PubSubController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpPost("Subscription1")]
    public async Task<IActionResult> HandleSubscription1()
    {
        using (StreamReader reader = new StreamReader(Request.Body))
            {
                var body = await reader.ReadToEndAsync();
                // Log or do something with the message here
                Console.WriteLine("\nA Product was inserted into MongoDB:");
                Console.WriteLine($"{body}");
            }

            // Respond with a 200 to acknowledge receipt of the message
            return Ok();
    }

    [HttpPost("InsertProduct")]
    public async Task<IActionResult> ExampleInsertProduct()
    {
        var product1 = new Product
        {
            Name = "Product 1",
            Description = "This is product 1",
            Price = 29.99m
        };

        await _productService.InsertAsync(product1);

        return Ok();
    }
}