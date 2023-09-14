using Microsoft.AspNetCore.Mvc;
using x_App.DomainAppServices;
using x_App.Infrastructure.Helpers.Attributes;
using x_Domain.DomainModels;

namespace x_endpoints.Controllers.REST;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ProductAppService _productAppService;

    public ProductsController(ProductAppService productAppService)
    {
        _productAppService = productAppService;
    }

    [HttpGet("GetEverything")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _productAppService.GetAllAsync();
        // Get the complete URL for this action.
        var completeUrl = $"{Request.Scheme}://{Request.Host}{Url.Action("GetAll", "Products")}";
        Console.WriteLine(completeUrl);  // This will print the full URL.

        return Ok(result);
    }

    // POST api/products
    [HttpPost("HandleProductUpdates")]
    [EventSubscription("local-x-service-ProductCreatedTopic")]
    public async Task<IActionResult> HandleProductUpdates([FromBody] Product product)
    {
        await _productAppService.InsertAsync(product);

        return Ok(product);
    }
}
