using Microsoft.Extensions.Hosting;
using Pipelines.Sockets.Unofficial.Arenas;

namespace DDD.Shop.Infrastructure.Messaging.RabbitMQ;

public abstract class MessageConsumer<T> : BackgroundService
{

    private readonly IRabbitMQClient _rabbitMqClient;
    private readonly string _queue;

    public MessageConsumer(IRabbitMQClient rabbitMqClient,string? queue = null)
    {
        _rabbitMqClient = rabbitMqClient;
        _queue = queue;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
       await _rabbitMqClient.Consumer<T>(_queue, async (data) =>
        {
            await HandleAsync(data,stoppingToken);
        });
        
    }
    
    protected abstract Task HandleAsync(T message,CancellationToken stoppingToken);
}