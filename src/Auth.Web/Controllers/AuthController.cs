using Microsoft.AspNetCore.Mvc;

namespace Auth.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login()
    {
        return Ok();
    }

    [HttpPost("register")]
    public IActionResult Register()
    {
        return Ok();
    }
}