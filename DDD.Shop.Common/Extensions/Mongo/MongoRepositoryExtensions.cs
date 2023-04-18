using System.Linq.Expressions;
using MongoDB.Driver;

namespace DDD.Shop.Common.Extensions.Mongo;

public static class MongoRepositoryExtensions
{
    public static void CreateIndex<T>(this IMongoCollection<T> collection, Expression<Func<T, object>> field)
    {
        var options = new CreateIndexOptions { Background = true };
        var indexDefinition = Builders<T>.IndexKeys.Ascending(field);
        var model = new CreateIndexModel<T>(indexDefinition, options);
        collection.Indexes.CreateOne(model);
    }
}