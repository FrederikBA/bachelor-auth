using Ardalis.Specification.EntityFrameworkCore;
using Auth.Core.Interfaces.Repositories;

namespace Auth.Infrastructure.Data;

public class EfReadRepository<T> : RepositoryBase<T>, IReadRepository<T> where T : class
{
    public readonly AuthContext AuthContext;
    
    public EfReadRepository(AuthContext dbContext) : base(dbContext)
    {
        AuthContext = dbContext;
    }
}