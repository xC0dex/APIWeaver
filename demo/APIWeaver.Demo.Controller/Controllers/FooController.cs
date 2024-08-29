using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace APIWeaver.ControllerApi.Demo.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = true)]
[Route("v{version:apiVersion}/foo")]
public class FooController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetUser()
    {
        var user = new UserDto
        {
            UserId = Guid.NewGuid(),
            Name = "dummy",
            Age = 69
        };
        return Ok(user);
    }
}