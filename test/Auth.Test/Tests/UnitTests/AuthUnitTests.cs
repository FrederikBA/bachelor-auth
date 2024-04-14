using Auth.Core.Entities;
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
}