using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Core.Entities;
using Auth.Core.Exceptions;
using Auth.Core.Interfaces.DomainServices;
using Auth.Core.Interfaces.Integration;
using Auth.Core.Interfaces.Repositories;
using Auth.Core.Models.Dtos;
using Auth.Core.Specifications;
using Microsoft.IdentityModel.Tokens;
using Shared.Integration.Configuration;
using Shared.Integration.Models.Dtos.Sync;

namespace Auth.Core.Services;

public class AuthService : IAuthService
{
    private readonly IRepository<User> _userRepository;
    private readonly ISyncProducer _syncProducer;

    public AuthService(IRepository<User> userRepository, ISyncProducer syncProducer)
    {
        _userRepository = userRepository;
        _syncProducer = syncProducer;
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
            RoleId = 2, //KemiDbUser
        };

        //Add user to database
        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();
        
        //Sync user with SEA database
        await _syncProducer.ProduceAsync(Config.Kafka.Topics.SyncAddUser, new SyncUserDto{UserId = user.Id, Username = user.Email});
        
        //Return userDto
        return user;
    }

    private string CreateToken(User user)
    {
        //Create claims
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role.RoleType.ToString())
        };

        //JWT secret key
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.Authorization.JwtKey));

        //Create token
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: cred
        );
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }
}