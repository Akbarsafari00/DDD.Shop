using System.Linq.Expressions;
using DDD.Shop.Common.Extensions.Mongo;
using DDD.Shop.Domain.Core;
using DDD.Shop.Domain.Core.Query;
using DDD.Shop.Domain.Core.ValueObjects;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace DDD.Shop.Infrastructure.DataAccess.Mongo;

public abstract class MongoRepository<TEntity> : IMongoRepository<TEntity> where TEntity : Entity
{
    private readonly IMongoCollection<TEntity> _collection;

    protected abstract void Configure(IMongoDatabase database, IMongoCollection<TEntity> collection);


    public MongoRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<TEntity>(typeof(TEntity).Name.ToLower());

        Configure(database, _collection);
    }

    public async Task<TEntity> GetByIdAsync(string id)
    {
        var filter = Builders<TEntity>.Filter.Eq(x => x.Id, id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public Task<TEntity> GetByIdAsync(BusinessId id)
    {
        return GetByIdAsync(id.ToString());
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _collection.Find(_=> true).ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filterExpression)
    {
        return await _collection.Find(filterExpression).ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> FindAsync(FilterDefinition<TEntity> filter)
    {
        
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> FindAsync(FilterDefinition<TEntity> filter, FindOptions<TEntity> options)
    {
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> FindAsync(string jsonQuery)
    {
        var filter = new JsonFilterDefinition<TEntity>(jsonQuery);
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> FindAsync(string jsonQuery, FindOptions<TEntity> options)
    {
        var filter = new JsonFilterDefinition<TEntity>(jsonQuery);
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> FindAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> FindAllAsync(FindOptions<TEntity> options)
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public async Task<PagedResult<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> filterExpression,
        int pageNumber, int pageSize)
    {
        var skip = (pageNumber - 1) * pageSize;
        var take = pageSize;

        var query = _collection.Find(filterExpression);

        var total = await query.CountDocumentsAsync();

        var entities = await query.Skip(skip).Limit(take).ToListAsync();

        return new PagedResult<TEntity>(entities, pageNumber, pageSize, (int)total);
    }


    public async Task<PagedResult<TEntity>> SearchAsync(FilterDefinition<TEntity> filter, int pageNumber, int pageSize)
    {
        var totalResults = await _collection.CountDocumentsAsync(filter);
        var options = new FindOptions<TEntity>
        {
            Skip = (pageNumber - 1) * pageSize,
            Limit = pageSize
        };

        var results = await _collection.FindAsync(filter, options);

        return new PagedResult<TEntity>
        {
            Results = await results.ToListAsync(),
            Page = pageNumber,
            PageSize = pageSize,
            TotalCount = totalResults,
        };
    }

    public async Task<PagedResult<TEntity>> SearchAsync(FilterDefinition<TEntity> filter, FindOptions<TEntity> options,
        int pageNumber, int pageSize)
    {
        var totalResults = await _collection.CountDocumentsAsync(filter);
        options.Skip = (pageNumber - 1) * pageSize;
        options.Limit = pageSize;

        var results = await _collection.FindAsync(filter, options);

        return new PagedResult<TEntity>
        {
            Results = await results.ToListAsync(),
            Page = pageNumber,
            PageSize = pageSize,
            TotalCount = totalResults,
        };
    }

    public async Task<PagedResult<TEntity>> SearchAsync(string jsonQuery, int pageNumber, int pageSize)
    {
        var filter = BsonSerializer.Deserialize<FilterDefinition<TEntity>>(jsonQuery);
        return await SearchAsync(filter, pageNumber, pageSize);
    }

    public async Task<PagedResult<TEntity>> SearchAsync(string jsonQuery, FindOptions<TEntity> options, int pageNumber,
        int pageSize)
    {
        var filter = BsonSerializer.Deserialize<FilterDefinition<TEntity>>(jsonQuery);
        return await SearchAsync(filter, options, pageNumber, pageSize);
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _collection.InsertOneAsync(entity);
        return entity;
    }

    public async Task<IEnumerable<TEntity>> CreateManyAsync(IEnumerable<TEntity> entities)
    {
        entities = entities.Select(x =>
        {
            x.CreatedAt = DateTime.UtcNow;
            return x;
        });
        await _collection.InsertManyAsync(entities);
        return entities;
    }

    public async Task<ReplaceOneResult> ReplaceOneAsync(string id, TEntity entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id);
        return await _collection.ReplaceOneAsync(filter, entity);
    }

    public Task<ReplaceOneResult> ReplaceOneAsync(BusinessId id, TEntity entity)
    {
        return ReplaceOneAsync(id.ToString(), entity);
    }

    public async Task<DeleteResult> DeleteAsync(string id)
    {
        var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id);
        return await _collection.DeleteOneAsync(filter);
    }

    public Task<DeleteResult> DeleteAsync(BusinessId id)
    {
        return DeleteAsync(id.ToString());
    }

    public async Task<DeleteResult> DeleteAsync(FilterDefinition<TEntity> filter)
    {
        return await _collection.DeleteManyAsync(filter);
    }

    public async Task<ReplaceOneResult> SoftDeleteAsync(string id)
    {
        var entity = await GetByIdAsync(id);
        entity.DeletedAt = DateTime.UtcNow;
        entity.IsDeleted = true;
        var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id);
        return await _collection.ReplaceOneAsync(filter, entity);
    }

    public Task<ReplaceOneResult> SoftDeleteAsync(BusinessId id)
    {
        return SoftDeleteAsync(id.ToString());
    }

   

    public async Task<long> CountAsync(Expression<Func<TEntity, bool>> filterExpression)
    {
        return await _collection.CountDocumentsAsync(filterExpression);
    }

    public async Task<long> CountAsync(FilterDefinition<TEntity> filter)
    {
        return await _collection.CountDocumentsAsync(filter);
    }

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filterExpression)
    {
        return await _collection.Find(filterExpression).AnyAsync();
    }

    public async Task<bool> ExistsAsync(FilterDefinition<TEntity> filter)
    {
        return await _collection.Find(filter).AnyAsync();
    }
}