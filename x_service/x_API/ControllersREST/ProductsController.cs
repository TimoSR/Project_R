using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using x_endpoints.DomainAppServices;
using x_lib.DomainModels;

namespace x_endpoints.ControllersREST;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ProductAppService _productAppService;

    public ProductsController(ProductAppService productAppService)
    {
        _productAppService = productAppService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _productAppService.GetAllAsync();
        return Ok(result);
    }

    // POST api/products
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Product product)
    {
        await _productAppService.InsertAsync(product);

        return Ok(product);
    }
}
