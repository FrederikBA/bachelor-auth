using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Auth.Core.Entities;
using Auth.Core.Exceptions;
using Auth.Core.Interfaces.DomainServices;
using Auth.Core.Interfaces.Repositories;
using Auth.Core.Models.Dtos;
using Auth.Core.Specifications;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Core.Services;

public class AuthService : IAuthService
{
    private readonly IRepository<User> _userRepository;

    public AuthService(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<string> LoginAsync(LoginDto dto)
    {
        var user = await _userRepository.FirstOrDefaultAsync(new GetUserByEmailWithRoleSpec(dto.Email));

        // validate
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            throw new LoginException("Username or password is incorrect");

        var token = CreateToken(user);

        return token;
    }

    public async Task<User> RegisterAsync(RegisterDto dto)
    {
        //Validate that email is not already in use
        var users = await _userRepository.ListAsync();

        if (users.Any(x => x.Email == dto.Email))
            throw new RegisterException("Email already exists");

        //Map dto to user and hash password
        var user = new User
        {
            Email = dto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            RoleId = 2 //KemiDbUser
        };

        //Add user to database
        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();
        
        //Return userDto
        return user;
    }

    private string CreateToken(User user)
    {
        // Create claims
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.RoleType.ToString())
        };

        // Generate a secure key
        var keyBytes = new byte[64]; // 512 bits
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(keyBytes);
        }

        var key = new SymmetricSecurityKey(keyBytes);

        // Create token
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: cred
        );
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }
}