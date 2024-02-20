using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace APIWeaver.ControllerApi.Demo.Controllers;

[ApiController]
[Route("[controller]")]
[ProducesResponseType<UserDto>(StatusCodes.Status404NotFound)]
[Obsolete]
public class UserController: ControllerBase
{
    
    // [HttpGet]
    // [ProducesResponseType<IEnumerable<UserDto>>(StatusCodes.Status200OK)]
    // // [Tags("uff")]
    // public IActionResult GetUsers()
    // {
    //     var users = Enumerable.Range(0, 9).Select(x => new UserDto
    //     {
    //         UserId = Guid.NewGuid(),
    //         Name = "Username",
    //         Age = x
    //     });
    //     return Ok(users);
    // }
    
    [HttpPost("{id:guid}")]
    [ProducesResponseType<UserDto>(StatusCodes.Status200OK, "may/content")]
    // [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
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