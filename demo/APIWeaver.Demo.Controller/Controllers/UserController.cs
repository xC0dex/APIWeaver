using System.ComponentModel.DataAnnotations;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIWeaver.ControllerApi.Demo.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = true)]
[Route("v{version:apiVersion}/users")]
public class UserController : ControllerBase
{
    [HttpPost("{id:guid}")]
    [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AllowAnonymous]
    public ActionResult<UserDto> GetUser(Guid id, [FromHeader] [Required] int age, [FromQuery] string? name)
    {
        var user = new UserDto
        {
            UserId = id,
            Name = name,
            Age = age
        };
        return Ok(user);
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<UserDto> GetUser()
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

public class UserDto
{
    public required Guid UserId { get; set; }

    public string? Name { get; set; }

    public int Age { get; set; }
}