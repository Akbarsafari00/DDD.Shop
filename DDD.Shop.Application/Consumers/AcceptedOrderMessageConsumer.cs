using DDD.Shop.Domain.Aggregates.OrderAggregate;
using DDD.Shop.Infrastructure.Messaging.RabbitMQ;

namespace DDD.Shop.Application.Consumers;

public class AcceptedOrderMessageConsumer : MessageConsumer<Order>
{
    public AcceptedOrderMessageConsumer(IRabbitMQClient rabbitMqClient) : base(rabbitMqClient , "Order:Accpted")
    {
    }

    protected override Task HandleAsync(Order message, CancellationToken stoppingToken)
    {
        Console.WriteLine("AcceptedOrderMessageConsumer : "+message.OrderItems.Count);
        return Task.CompletedTask;
    }
}