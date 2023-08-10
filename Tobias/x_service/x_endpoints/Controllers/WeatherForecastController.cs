using Microsoft.AspNetCore.Mvc;
using x_endpoints.Models;
using x_endpoints.Services;

namespace x_endpoints.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    
    private readonly ProductService _productService;

    public WeatherForecastController(ProductService productService, ILogger<WeatherForecastController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<WeatherForecast[]> Get()
    {
        var product1 = new Product
        {
            Name = "Product 1",
            Description = "This is product 1",
            Price = 29.99m
        };

        await _productService.InsertProduct(product1);
        
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
