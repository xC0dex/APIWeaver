using Microsoft.AspNetCore.Mvc;

namespace APIWeaver.OpenApi.ControllerApi.Controllers;

[ApiController]
[Route("[controller]")]
[ProducesResponseType<User>(StatusCodes.Status500InternalServerError)]
public class UserController: ControllerBase
{
    [ProducesResponseType<User>(StatusCodes.Status200OK)]
    public IActionResult GetUser(User user)
    {
        return Ok(user);
    }
}