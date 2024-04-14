using Auth.Core.Interfaces.DomainServices;
using Auth.Core.Models.Dtos;
using Auth.Web.Interfaces;
using Auth.Web.Models;

namespace Auth.Web.Services;

public class ViewModelService : IViewModelService
{
    private readonly IAuthService _authService;

    public ViewModelService(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<UserViewModel> MapRegisterUser(RegisterDto dto)
    {
        var user = await _authService.RegisterAsync(dto);
        
        // Map the user to the view model
        var userViewModel = new UserViewModel
        {
            Email = user.Email,
            Role = "KemiDbUser"
        };
        
        return userViewModel;
    }
}