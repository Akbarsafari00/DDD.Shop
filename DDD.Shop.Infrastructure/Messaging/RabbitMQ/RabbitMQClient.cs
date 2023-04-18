using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DDD.Shop.Infrastructure.Messaging.RabbitMQ;

public class RabbitMQClient : IRabbitMQClient
{
    private readonly IModel _channel;
    private readonly RabbitMQOptions _mqOptions;
    public RabbitMQClient(IOptions<RabbitMQOptions> options)
    {
        _mqOptions = options.Value;
        var factory = new ConnectionFactory
        {
            HostName = options.Value.Hostname,
            UserName = options.Value.Username,
            Password = options.Value.Password,
            Port = options.Value.Port
        };

        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();
        _channel.ExchangeDeclare("Message:Sender", ExchangeType.Direct, durable: true,autoDelete:true);
        _channel.ExchangeDeclare("Message:Publisher", ExchangeType.Direct, durable: true,autoDelete:true);
        _channel.ExchangeDeclare("Message:Broadcaster", ExchangeType.Fanout, durable: true,autoDelete:true);
    }

    public  Task SendAsync<T>( string queue,T message, string? messageId = null, string? correlationId = null)
    {
        try
        {
           
            return Task.Run(() =>
            {
                var data= JsonConvert.SerializeObject(message,new JsonSerializerSettings(){ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor});
            
                var body = Encoding.UTF8.GetBytes(data);

                var properties = _channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.MessageId = messageId ?? Guid.NewGuid().ToString();
                properties.CorrelationId = correlationId;
                _channel.BasicPublish("Message:Sender", $"{queue}", properties, body);
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public  Task PublishAsync<T>(T message, string messageId = null, string correlationId = null)
    {
        try
        {
           return Task.Run(() =>
            {
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                var properties = _channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.MessageId = messageId ?? Guid.NewGuid().ToString();
                properties.CorrelationId = correlationId;
                _channel.BasicPublish("Message:Publisher", "#", properties, body);
            });

           
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task BroadcastAsync<T>(T message, string messageId = null, string correlationId = null)
    {
        try
        {
            return Task.Run(() =>
            {
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                var properties = _channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.MessageId = messageId ?? Guid.NewGuid().ToString();
                properties.CorrelationId = correlationId;
                _channel.BasicPublish("Message:Broadcaster","#" , properties, body);
            });

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task Consumer<T>(string queueName, Func<T, Task> onMessageReceived)
    {

        return Task.Run(() =>
        {
            _channel.QueueDeclare(queueName, true, false, false);
            _channel.QueueBind(queueName, "Message:Publisher", "#");

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var message = JsonConvert.DeserializeObject<T>(json);

                await onMessageReceived(message);

                _channel.BasicAck(ea.DeliveryTag, multiple: false);
            };

            _channel.BasicConsume(queueName, autoAck: false, consumer: consumer);
        });
    }
    public Task Receiver<T>(string queueName, Func<T, Task> onMessageReceived)
    {
       return Task.Run(() =>
       {
           _channel.QueueDeclare(queueName, true, false, false);
           _channel.QueueBind(queueName,"Message:Sender",queueName);
        
           var consumer = new EventingBasicConsumer(_channel);

           consumer.Received += async (model, ea) =>
           {
               var body = ea.Body.ToArray();
               var json = Encoding.UTF8.GetString(body);
               var message = JsonConvert.DeserializeObject<T>(json);
            
               await onMessageReceived(message);

               _channel.BasicAck(ea.DeliveryTag, multiple: false);
           };

           _channel.BasicConsume(queueName, autoAck: false, consumer: consumer);
       });
    }

    public Task Subscriber<T>(string queueName, Func<T, Task> onMessageReceived)
    {
       return  Task.Run(() =>
       {
           _channel.QueueDeclare(queueName, true, false, false);
           _channel.QueueBind(queueName,"Message:Broadcaster","#");
        
           var consumer = new EventingBasicConsumer(_channel);

           consumer.Received += async (model, ea) =>
           {
               var body = ea.Body.ToArray();
               var json = Encoding.UTF8.GetString(body);
               var message = JsonConvert.DeserializeObject<T>(json);

               await onMessageReceived(message);

               _channel.BasicAck(ea.DeliveryTag, multiple: false);
           };

           _channel.BasicConsume(queueName, autoAck: false, consumer: consumer);
       });
    }
}