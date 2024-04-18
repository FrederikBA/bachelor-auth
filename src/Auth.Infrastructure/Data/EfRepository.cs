using Ardalis.Specification.EntityFrameworkCore;
using Auth.Core.Interfaces.Repositories;
using Shared.Integration.Interfaces;

namespace Auth.Infrastructure.Data;

public class EfRepository<T> : RepositoryBase<T>, IRepository<T> where T : class, IAggregateRoot
{
    public readonly AuthContext AuthContext;
    
    public EfRepository(AuthContext dbContext) : base(dbContext)
    {
        AuthContext = dbContext;
    }
}