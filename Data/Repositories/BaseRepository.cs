using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using Data.Helpers;
using System.Diagnostics;
using Data.Interfaces;


namespace Data.Repositories;

/// <summary>
/// Base repository for standard database operations.
/// Returns RepositoryResult objects to support business-layer error handling.
/// </summary>
public abstract class BaseRepository<TEntity>(DataContext context) : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly DataContext _context = context;
    protected readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();
    private IDbContextTransaction? _transaction = null;

    #region Transaction Management

    public virtual async Task BeginTransactionAsync()
    {
        _transaction ??= await _context.Database.BeginTransactionAsync();
    }

    public virtual async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public virtual async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    #endregion

    #region CRUD

    public virtual async Task<RepositoryResult<TEntity>> AddAsync(TEntity entity)
    {
        try
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();

            return RepositoryResult<TEntity>.Success(entity);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error adding {nameof(TEntity)}: {ex.Message}");
            return RepositoryResult<TEntity>.Failure($"Failed to add entity: {ex.Message}", 500);
        }
    }

    /// <summary>
    /// Returns all entities of type TEntity.
    /// </summary>
    public virtual async Task<RepositoryResult<IEnumerable<TEntity>>> GetAllAsync()
    {
        try
        {
            var items = await _dbSet.ToListAsync();
            return RepositoryResult<IEnumerable<TEntity>>.Success(items);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error retrieving all {nameof(TEntity)}: {ex.Message}");
            return RepositoryResult<IEnumerable<TEntity>>.Failure($"Failed to retrieve entities: {ex.Message}", 500);
        }
    }

    /// <summary>
    /// Returns entities projected to TSelect, with optional filtering, sorting, and includes.
    /// </summary>
    public virtual async Task<RepositoryResult<IEnumerable<TSelect>>> GetAllAsync<TSelect>(
        Expression<Func<TEntity, TSelect>> selector,
        bool orderByDescending = false,
        Expression<Func<TEntity, object>>? sortBy = null,
        Expression<Func<TEntity, bool>>? where = null,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet;

        if (where != null)
            query = query.Where(where);

        if (includes?.Length > 0)
            foreach (var include in includes)
                query = query.Include(include);

        if (sortBy != null)
            query = orderByDescending
                ? query.OrderByDescending(sortBy)
                : query.OrderBy(sortBy);

        var projected = await query
            .Select(selector)
            .ToListAsync();

        return new RepositoryResult<IEnumerable<TSelect>>
        {
            Succeeded = true,
            StatusCode = 200,
            Result = projected
        };
    }

    /// <summary>
    /// Retrieves all entities including related navigation properties.
    /// </summary>
    public virtual async Task<IEnumerable<TEntity>> GetAllWithDetailsAsync(
        Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression)
    {
        try
        {
            IQueryable<TEntity> query = _dbSet;
            if (includeExpression != null)
                query = includeExpression(query);

            return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error retrieving all {nameof(TEntity)} with details: {ex.Message}");
            return Enumerable.Empty<TEntity>();
        }
    }

    /// <summary>
    /// Retrieves a single entity matching the given predicate.
    /// </summary>
    public virtual async Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>> expression)
    {
        try
        {
            return await _dbSet.FirstOrDefaultAsync(expression);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error retrieving {nameof(TEntity)}: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Retrieves a single entity matching the predicate, including related data.
    /// </summary>
    public virtual async Task<TEntity?> GetOneWithDetailsAsync(
        Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression,
        Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            IQueryable<TEntity> query = _dbSet;
            if (includeExpression != null)
                query = includeExpression(query);

            return await query.FirstOrDefaultAsync(predicate);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error retrieving {nameof(TEntity)} with details: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Marks the entity as modified in the context.
    /// </summary>
    public virtual async Task<RepositoryResult<TEntity>> UpdateAsync(TEntity entity)
    {
        try
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();

            return RepositoryResult<TEntity>.Success(entity);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating {nameof(TEntity)}: {ex.Message}");
            return RepositoryResult<TEntity>.Failure($"Failed to update entity: {ex.Message}", 500);
        }
    }

    /// <summary>
    /// Marks the entity for deletion from the context.
    /// </summary>
    public virtual async Task<RepositoryResult<TEntity>> DeleteAsync(TEntity entity)
    {
        try
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();

            return RepositoryResult<TEntity>.Success(entity);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error deleting {nameof(TEntity)}: {ex.Message}");
            return RepositoryResult<TEntity>.Failure($"Failed to delete entity: {ex.Message}", 500);
        }
    }

    /// <summary>
    /// Checks if any entity matches the given predicate.
    /// </summary>
    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            return await _dbSet.AnyAsync(predicate);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error checking existence of {nameof(TEntity)}: {ex.Message}");
            return false;
        }
    }

    #endregion
}
