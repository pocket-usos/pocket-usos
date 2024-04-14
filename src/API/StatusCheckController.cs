using Microsoft.AspNetCore.Mvc;

namespace App.API;

[ApiController]
public class StatusCheckController : ControllerBase
{
    [HttpGet("ping")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public IActionResult Ping()
    {
        return Ok("pong");
    }
}
