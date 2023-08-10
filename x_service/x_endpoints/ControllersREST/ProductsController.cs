using Microsoft.AspNetCore.Mvc;
using x_endpoints.ControllerServices;
using x_endpoints.Models;

namespace x_endpoints.ControllersREST;

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
