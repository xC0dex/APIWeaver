using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace APIWeaver.ControllerApi.Demo.Controllers;

[ApiController]
[ApiVersion(1)]
[Route("v{version:apiVersion}/[controller]")]
public class BookController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public IActionResult GetBook()
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