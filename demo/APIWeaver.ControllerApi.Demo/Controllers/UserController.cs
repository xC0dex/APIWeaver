using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace APIWeaver.ControllerApi.Demo.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController: ControllerBase
{
    
    [HttpPost("{id:guid}")]
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

public class UserDto
{
    public required Guid UserId { get; set; }

    public string? Name { get; set; }

    public int Age { get; set; }
}