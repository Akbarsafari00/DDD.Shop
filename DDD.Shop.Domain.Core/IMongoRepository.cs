using System.Linq.Expressions;
using DDD.Shop.Domain.Core.Query;
using DDD.Shop.Domain.Core.ValueObjects;
using MongoDB.Driver;

namespace DDD.Shop.Domain.Core;

public interface IMongoRepository<TEntity> where TEntity : Entity
{
    
    Task<TEntity> GetByIdAsync(string id);
    Task<TEntity> GetByIdAsync(BusinessId id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filterExpression);
    Task<IEnumerable<TEntity>> FindAsync(FilterDefinition<TEntity> filter);
    Task<IEnumerable<TEntity>> FindAsync(FilterDefinition<TEntity> filter, FindOptions<TEntity> options);
    Task<IEnumerable<TEntity>> FindAsync(string jsonQuery);
    Task<IEnumerable<TEntity>> FindAsync(string jsonQuery, FindOptions<TEntity> options);
    Task<IEnumerable<TEntity>> FindAllAsync();
    Task<IEnumerable<TEntity>> FindAllAsync(FindOptions<TEntity> options);
    Task<PagedResult<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> filterExpression, int pageNumber, int pageSize);
    Task<PagedResult<TEntity>> SearchAsync(FilterDefinition<TEntity> filter, int pageNumber, int pageSize);
    Task<PagedResult<TEntity>> SearchAsync(FilterDefinition<TEntity> filter, FindOptions<TEntity> options, int pageNumber, int pageSize);
    Task<PagedResult<TEntity>> SearchAsync(string jsonQuery, int pageNumber, int pageSize);
    Task<PagedResult<TEntity>> SearchAsync(string jsonQuery, FindOptions<TEntity> options, int pageNumber, int pageSize);
    Task<TEntity> CreateAsync(TEntity entity);
    Task<IEnumerable<TEntity>> CreateManyAsync(IEnumerable<TEntity> entities);
    Task<ReplaceOneResult> ReplaceOneAsync(string id, TEntity entity);
    Task<ReplaceOneResult> ReplaceOneAsync(BusinessId id, TEntity entity);
    Task<DeleteResult> DeleteAsync(string id);
    Task<DeleteResult> DeleteAsync(BusinessId id);
    Task<DeleteResult> DeleteAsync(FilterDefinition<TEntity> filter);
    Task<ReplaceOneResult> SoftDeleteAsync(string id);
    Task<ReplaceOneResult> SoftDeleteAsync(BusinessId id);
    Task<long> CountAsync(Expression<Func<TEntity, bool>> filterExpression);
    Task<long> CountAsync(FilterDefinition<TEntity> filter);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filterExpression);
    Task<bool> ExistsAsync(FilterDefinition<TEntity> filter);
}
