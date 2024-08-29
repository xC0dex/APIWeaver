using System.ComponentModel.DataAnnotations;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace APIWeaver.ControllerApi.Demo.Controllers;

[ApiController]
[ApiVersion(2)]
[Route("v{version:apiVersion}/users")]
public class UserControllerNew : ControllerBase
{
    [HttpPost("dummy/{id:guid}")]
    [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<UserDto> GetUser(Guid id, [FromHeader] [Required] int age, [FromQuery] string? name, [FromBody] UserDto userDto)
    {
        var user = new UserDto
        {
            UserId = id,
            Name = name,
            Age = age
        };
        return Ok(user);
    }
}