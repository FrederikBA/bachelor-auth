using Ardalis.Specification;

namespace Auth.Core.Interfaces.Repositories;

public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
{
}