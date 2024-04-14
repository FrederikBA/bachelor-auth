using Auth.Core.Models.Dtos;
using Auth.Web.Models;

namespace Auth.Web.Interfaces;

public interface IViewModelService
{
    Task<UserViewModel> MapRegisterUser(RegisterDto dto);
}