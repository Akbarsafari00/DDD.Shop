using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DDD.Shop.Infrastructure.Messaging.RabbitMQ;

public static class Extensions
{
    public static IServiceCollection AddRabbitMQ(
        this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider().CreateScope().ServiceProvider;
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        
        services.Configure<RabbitMQOptions>(configuration.GetSection("RabbitMQ"));
        services.AddSingleton<IRabbitMQClient, RabbitMQClient>();
        return services;
    }
}