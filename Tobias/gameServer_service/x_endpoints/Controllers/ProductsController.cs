using Microsoft.AspNetCore.Mvc;
using x_endpoints.Models;
using x_endpoints.Services;

namespace x_endpoints.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductsController(ProductService productService)
    {
        _productService = productService;
    }

    // POST api/products
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Product product)
    {
        await _productService.InsertProduct(product);
        
        return Ok(product);
    }

    // Other action methods...
}
