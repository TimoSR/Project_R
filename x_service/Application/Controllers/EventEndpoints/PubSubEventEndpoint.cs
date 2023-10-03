using Application.AppServices;
using Domain.DomainModels;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers.EventEndpoints;

[ApiController]
[Route("api/[controller]")]
public class PubSubEventEndpoint : ControllerBase
{
    private readonly IProductService _productService;

    public PubSubEventEndpoint(IProductService productService)
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
            Price = 29.99
        };

        await _productService.InsertAsync(product1);
        
        // This dynamically gets actual host
        // Get the complete URL for this action.
        var completeUrl = $"{Request.Scheme}://{Request.Host}{Url.Action("ExampleInsertProduct", "PubSubEventEndpoint")}";
        Console.WriteLine(completeUrl);  // This will print the full URL for the "InsertProduct" action.

        return Ok();
    }
}