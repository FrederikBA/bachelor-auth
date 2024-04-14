using Ardalis.Specification;
using Auth.Core.Entities;

namespace Auth.Core.Specifications;

public sealed class GetUserByEmailWithRoleSpec : Specification<User>
{
    public GetUserByEmailWithRoleSpec(string email)
    {
        Query.Where(user => user.Email == email);
        Query.Include(user => user.Role);
    }
}