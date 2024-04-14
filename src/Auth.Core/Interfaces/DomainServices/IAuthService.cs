using Auth.Core.Entities;
using Auth.Core.Models.Dtos;

namespace Auth.Core.Interfaces.DomainServices;

public interface IAuthService
{
    Task<string> LoginAsync(LoginDto dto);
    Task<User> RegisterAsync(RegisterDto dto);
}