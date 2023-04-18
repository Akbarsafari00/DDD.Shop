namespace DDD.Shop.Infrastructure.Messaging.RabbitMQ;

public class RabbitMQOptions
{
    public string Hostname { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    
    public string VirtualHost { get; set; }
}