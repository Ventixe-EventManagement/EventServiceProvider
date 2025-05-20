using Data.Helpers;
using System.Linq.Expressions;

namespace Data.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : class
{
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();

    Task<RepositoryResult<TEntity>> AddAsync(TEntity entity);
    Task<RepositoryResult<TEntity>> UpdateAsync(TEntity entity);
    Task<RepositoryResult<TEntity>> DeleteAsync(TEntity entity);
    Task<RepositoryResult<IEnumerable<TEntity>>> GetAllAsync();
    Task<IEnumerable<TEntity>> GetAllWithDetailsAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression);
    Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>> expression);
    Task<TEntity?> GetOneWithDetailsAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression, Expression<Func<TEntity, bool>> predicate);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);

    Task<RepositoryResult<IEnumerable<TSelect>>> GetAllAsync<TSelect>(
        Expression<Func<TEntity, TSelect>> selector,
        bool orderByDescending = false,
        Expression<Func<TEntity, object>>? sortBy = null,
        Expression<Func<TEntity, bool>>? where = null,
        params Expression<Func<TEntity, object>>[] includes);
}