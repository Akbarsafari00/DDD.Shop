using DDD.Shop.Domain.Aggregates.OrderAggregate;
using DDD.Shop.Infrastructure.Messaging.RabbitMQ;

namespace DDD.Shop.Application.Consumers;

public class PendingOrderMessageConsumer : MessageConsumer<Order>
{
    public PendingOrderMessageConsumer(IRabbitMQClient rabbitMqClient) : base(rabbitMqClient , "Order:Pending")
    {
    }

    protected override Task HandleAsync(Order message, CancellationToken stoppingToken)
    {
        Console.WriteLine("PendingOrderMessageConsumer : "+message.Email);
        return Task.CompletedTask;
    }
}