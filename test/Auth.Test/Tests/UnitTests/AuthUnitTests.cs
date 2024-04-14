using Auth.Core.Entities;
using Auth.Core.Interfaces.DomainServices;
using Auth.Core.Interfaces.Repositories;
using Auth.Core.Models.Dtos;
using Auth.Core.Services;
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
    public void LoginAsync_WhenCalled_ReturnsString()
    {
        // Arrange
        var dto = new LoginDto();
        
        // Act
        var result = _authService.LoginAsync(dto);
        
        // Assert
        Assert.NotNull(result);
    }
}