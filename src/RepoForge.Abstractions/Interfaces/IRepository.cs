using System.Linq.Expressions;

namespace RepoForge.Abstractions;

/// <summary>
/// Generic repository abstraction that exposes common CRUD and query operations
/// for entities of type <typeparamref name="T"/>.
/// </summary>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Finds an entity by its key(s).
    /// </summary>
    /// <param name="keys">Key values (e.g., primary key or composite key).</param>
    /// <returns>The entity if found; otherwise <c>null</c>.</returns>
    Task<T?> GetByIdAsync(params object[] keys);

    /// <summary>
    /// Retrieves all entities.
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Adds a new entity to the underlying store.
    /// </summary>
    /// <param name="entity">The entity instance to add.</param>
    Task AddAsync(T entity);

    /// <summary>
    /// Updates an existing entity in the underlying store.
    /// </summary>
    /// <param name="entity">The entity instance to update.</param>
    Task UpdateAsync(T entity);

    /// <summary>
    /// Deletes an entity identified by its key(s).
    /// </summary>
    /// <param name="keys">Key values for the entity to delete.</param>
    Task DeleteAsync(params object[] keys);

    /// <summary>
    /// Finds entities that match the provided predicate.
    /// </summary>
    /// <param name="predicate">Filter predicate.</param>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
}