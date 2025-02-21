using ModularMonolith.Shared.Abstractions.Domain;

namespace ModularMonolith.Shared.Abstractions.Repository;

public interface IRepository<TEntity> where TEntity : AggregateRoot
{
    Task<TEntity> GetByIdAsync(Guid id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
}
