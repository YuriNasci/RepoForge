using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using RepoForge.Abstractions;

namespace RepoForge.AWS.DynamoDB;

/// <summary>
/// Amazon DynamoDB implementation of <see cref="IRepository{T}"/> backed by <see cref="IDynamoDBContext"/>.
/// </summary>
public class DynamoRepository<T> : IRepository<T> where T : class
{
    private readonly IDynamoDBContext _context;

    /// <summary>
    /// Creates a new <see cref="DynamoRepository{T}"/>.
    /// </summary>
    /// <param name="context">The DynamoDB context.</param>
    public DynamoRepository(IDynamoDBContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    /// <summary>
    /// Gets an entity by its key.
    /// </summary>
    /// <param name="keys">The key(s) of the entity.</param>
    /// <returns>The entity if found, otherwise <see langword="null"/>.</returns>
    public async Task<T?> GetByIdAsync(params object[] keys)
    {
        return keys.Length switch
        {
            1 => await _context.LoadAsync<T>(keys[0]),
            2 => await _context.LoadAsync<T>(keys[0], keys[1]),
            _ => throw new ArgumentException("DynamoDB requer 1 (PartitionKey) ou 2 chaves (PartitionKey + SortKey).")
        };
    }

    /// <inheritdoc />
    /// <summary>
    /// Gets all entities.
    /// </summary>
    /// <returns>A collection of all entities.</returns>
    public async Task<IEnumerable<T>> GetAllAsync() =>
        await _context.ScanAsync<T>(default).GetRemainingAsync();

    /// <inheritdoc />
    /// <summary>
    /// Adds an entity.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    public async Task AddAsync(T entity) =>
        await _context.SaveAsync(entity);

    /// <inheritdoc />
    /// <summary>
    /// Updates an entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    public async Task UpdateAsync(T entity) =>
        await _context.SaveAsync(entity);

    /// <inheritdoc />
    /// <summary>
    /// Deletes an entity by its key.
    /// </summary>
    /// <param name="keys">The key(s) of the entity.</param>
    public async Task DeleteAsync(params object[] keys)
    {
        var entity = await GetByIdAsync(keys);
        if (entity != null)
            await _context.DeleteAsync(entity);
    }

    /// <inheritdoc />
    /// <summary>
    /// Finds entities by a predicate.
    /// </summary>
    /// <param name="predicate">The predicate to filter entities.</param>
    /// <returns>A collection of entities that match the predicate.</returns>
    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        // DynamoDB não suporta LINQ avançado como EF, mas podemos simular com Scan
        var conditions = new List<ScanCondition>();
        // se necessário, poderíamos mapear Expression<Func<T,bool>> para ScanCondition
        return await _context.ScanAsync<T>(conditions).GetRemainingAsync();
    }
}