using Auth.Core.Entities;

namespace Auth.Test.Helpers;

public static class AuthTestHelper
{
    public static User GetTestUser()
    {
        return new User
        {
            Id = 1,
            Email = "test@example.com",
            Password = BCrypt.Net.BCrypt.HashPassword("testpassword"),
            RoleId = 2,
            Role = new Role
            {
                RoleType = RoleTypes.KemiDbUser
            }
        };
    }
}