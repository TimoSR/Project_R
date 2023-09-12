using Microsoft.AspNetCore.Mvc;
using x_endpoints.DomainAppServices;
using x_lib.DomainModels;

namespace x_endpoints.ControllersREST;

[ApiController]
[Route("api/[controller]")]
public class PubSubController : ControllerBase
{
    private readonly ProductAppService _productAppService;

    public PubSubController(ProductAppService productAppService)
    {
        _productAppService = productAppService;
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
            Price = 29.99
        };

        await _productAppService.InsertAsync(product1);
        
        // This dynamically gets actual host
        // Get the complete URL for this action.
        var completeUrl = $"{Request.Scheme}://{Request.Host}{Url.Action("ExampleInsertProduct", "PubSub")}";
        Console.WriteLine(completeUrl);  // This will print the full URL for the "InsertProduct" action.

        return Ok();
    }
}