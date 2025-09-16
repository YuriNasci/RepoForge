using System.Linq.Expressions;

namespace RepoForge.Abstractions.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);

    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
}