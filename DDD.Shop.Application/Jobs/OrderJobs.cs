using DDD.Shop.Application.Models.Storage;
using DDD.Shop.Common.BackgroundServices;
using DDD.Shop.Domain.Aggregates.OrderAggregate;
using DDD.Shop.Domain.Core.ValueObjects;
using DDD.Shop.Domain.Repositories;
using DDD.Shop.Infrastructure.DataAccess.Redis;
using DDD.Shop.Infrastructure.Messaging.RabbitMQ;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace DDD.Shop.Application.Jobs;

public class OrderJobs : RapidBackgroundService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IRabbitMQClient _rabbitMqClient;
    private readonly IRedisCached<OrderStorage> _cachedOrderStorage;
    public OrderJobs(ILogger<RapidBackgroundService> logger, IOrderRepository orderRepository,
        IRedisCached<OrderStorage> cachedOrderStorage, IRabbitMQClient rabbitMqClient) : base(logger, TimeSpan.FromSeconds(50))
    {
        _orderRepository = orderRepository;
        _cachedOrderStorage = cachedOrderStorage;
        _rabbitMqClient = rabbitMqClient;
    }

    protected override async Task HandleAsync(CancellationToken stoppingToken)
    {

        var order = await _orderRepository.FindAllAsync();
        
        await _rabbitMqClient.PublishAsync<Order>(order.FirstOrDefault());
    }
}