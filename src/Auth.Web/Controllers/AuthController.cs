using Auth.Core.Interfaces.DomainServices;
using Auth.Core.Models.Dtos;
using Auth.Web.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IViewModelService _viewModelService;

    public AuthController(IAuthService authService, IViewModelService viewModelService)
    {
        _authService = authService;
        _viewModelService = viewModelService;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginDto dto)
    {
        try
        {
            var token = await _authService.LoginAsync(dto);
            return Ok(token);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterDto dto)
    {
        try
        {
            var user = await _viewModelService.MapRegisterUser(dto);
            return Ok(user);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}