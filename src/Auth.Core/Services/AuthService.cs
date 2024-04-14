using Auth.Core.Entities;
using Auth.Core.Interfaces.DomainServices;
using Auth.Core.Interfaces.Repositories;
using Auth.Core.Models.Dtos;

namespace Auth.Core.Services;

public class AuthService : IAuthService
{
    private readonly IRepository<User> _userRepository;

    public AuthService(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<string> LoginAsync(LoginDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<User> RegisterAsync(RegisterDto dto)
    {
        throw new NotImplementedException();
    }
}