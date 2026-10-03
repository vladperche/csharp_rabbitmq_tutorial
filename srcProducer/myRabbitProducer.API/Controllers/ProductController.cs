
using Microsoft.AspNetCore.Mvc;
using myRabbitProducer.Entities.Interfaces;
using myRabbitProducer.Entities.Models;

namespace myRabbitProducer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly IRabbitClient _rabbitClient;

    public ProductController(ILogger<ProductController> logger, IRabbitClient rabbitClient)
    {
        _logger = logger;
        _rabbitClient = rabbitClient;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateProduct([FromBody] ProductModel product)
    {
        if (product == null)
        {
            _logger.LogCritical("Received null product data.");
            return BadRequest("Product data is required.");
        }

        _logger.LogDebug("Received product data: {@Product}", product);
        await _rabbitClient.PublishAsync(product);
        _logger.LogInformation("Product published successfully: {@Product}", product);
        
        return Ok("Product published successfully.");
    }
}
