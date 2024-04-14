using Auth.Core.Entities;
using Auth.Core.Exceptions;
using Auth.Core.Interfaces.DomainServices;
using Auth.Core.Interfaces.Repositories;
using Auth.Core.Models.Dtos;
using Auth.Core.Services;
using Auth.Core.Specifications;
using Auth.Test.Helpers;
using Moq;

namespace Auth.Test.Tests.UnitTests;

public class AuthUnitTests
{
    private readonly IAuthService _authService;
    private readonly Mock<IRepository<User>> _userRepositoryMock = new();
    
    public AuthUnitTests()
    {
        _authService = new AuthService(_userRepositoryMock.Object);
    }
    
    [Fact]
    public async Task LoginAsync_WhenCalled_ReturnsString()
    {
        // Arrange
        var user = AuthTestHelper.GetTestUser();
        var userDto = new LoginDto
        {
            Email = user.Email,
            Password = "testpassword"
        };
        
        _userRepositoryMock.Setup(x => x.FirstOrDefaultAsync(It.IsAny<GetUserByEmailWithRoleSpec>(), new CancellationToken()))
            .ReturnsAsync(user);
        
        // Act
        var result = await _authService.LoginAsync(userDto);
        
        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task LoginAsync_WhenCalled_ThrowsLoginException()
    {
      // Arrange
      var user = AuthTestHelper.GetTestUser();
      var userDto = new LoginDto
      {
          Email = user.Email,
          Password = "wrongpassword"
      };
      
      _userRepositoryMock.Setup(x => x.FirstOrDefaultAsync(It.IsAny<GetUserByEmailWithRoleSpec>(), new CancellationToken()))
          .ReturnsAsync(user);
      
      //Act + Assert
      await Assert.ThrowsAsync<LoginException>( () => _authService.LoginAsync(userDto));
    }

    [Fact]
    public async Task RegisterAsync_WhenCalled_ReturnsUser()
    {
        //Arrange
        var dto = new RegisterDto
        {
            Email = "test@example.com",
            Password = "password"
        };
        
        //Mock users to ensure that the repository returns an empty list rather than null
        _userRepositoryMock.Setup(x => x.ListAsync(new CancellationToken()))
            .ReturnsAsync(new List<User>());
        
        //Act
        var result = await _authService.RegisterAsync(dto);
        
        //Assert
        Assert.Equal(2, result.RoleId); //Is KemiDbUser role
        Assert.Equal(dto.Email, result.Email); //Email is correct
    }

    [Fact]
    public async Task RegisterAsync_WhenCalled_ThrowsRegisterException()
    {
        //Arrange
        var dto = new RegisterDto
        {
            Email = "test@example.com",
            Password = "password"
        };
        
        //Mock users to ensure that the repository returns a list with a user with the same email as dto
        _userRepositoryMock.Setup(x => x.ListAsync(new CancellationToken()))
            .ReturnsAsync(new List<User> { new User { Email = dto.Email } });
        
        //Act + Assert
        await Assert.ThrowsAsync<RegisterException>(() => _authService.RegisterAsync(dto));
    }

}