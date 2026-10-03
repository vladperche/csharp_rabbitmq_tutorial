
using Microsoft.AspNetCore.Mvc;

namespace myRabbitProducer.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet("check")]
    public IActionResult Get()
    {
        return Ok(new { status = "Running..." });
    }
}
