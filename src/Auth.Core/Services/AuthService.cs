using Auth.Core.Entities;
using Auth.Core.Interfaces.DomainServices;
using Auth.Core.Models.Dtos;

namespace Auth.Core.Services;

public class AuthService : IAuthService
{
    public Task<string> LoginAsync(LoginDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<User> RegisterAsync(RegisterDto dto)
    {
        throw new NotImplementedException();
    }
}