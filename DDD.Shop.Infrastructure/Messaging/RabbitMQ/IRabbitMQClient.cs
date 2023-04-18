namespace DDD.Shop.Infrastructure.Messaging.RabbitMQ;

public interface IRabbitMQClient
{
    Task SendAsync<T>(string queue,T message,  string? messageId = null, string correlationId = null);
    Task PublishAsync<T>(T message, string messageId = null, string correlationId = null);
    Task BroadcastAsync<T>(T message, string messageId = null, string correlationId = null);
    Task Consumer<T>(string queueName, Func<T, Task> onMessageReceived);
    Task Receiver<T>(string queueName, Func<T, Task> onMessageReceived);
    Task Subscriber<T>(string queueName, Func<T, Task> onMessageReceived);
}