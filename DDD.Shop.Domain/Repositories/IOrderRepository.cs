using DDD.Shop.Domain.Aggregates.OrderAggregate;
using DDD.Shop.Domain.Core;
using DDD.Shop.Domain.Core.ValueObjects;

namespace DDD.Shop.Domain.Repositories;

public interface IOrderRepository : IMongoRepository<Order>
{
    
}