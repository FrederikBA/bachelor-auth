using Ardalis.Specification;

namespace Auth.Core.Interfaces.Repositories;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class
{
    
}