using DDD.Shop.Common.Extensions.Mongo;
using DDD.Shop.Domain.Aggregates.OrderAggregate;
using DDD.Shop.Domain.Core;
using DDD.Shop.Domain.Core.ValueObjects;
using DDD.Shop.Domain.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DDD.Shop.Infrastructure.DataAccess.Mongo.Repositories;

public class OrderRepository :  MongoRepository<Order>,IOrderRepository 
{
    public OrderRepository(IMongoDatabase database) : base(database)
    {
    }


    protected override void Configure(IMongoDatabase database, IMongoCollection<Order> collection)
    {
        
       collection.CreateIndex(x=>x.Email);
    }
}