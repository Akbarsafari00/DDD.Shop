using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DDD.Shop.Infrastructure.DataAccess.Redis;

public static class Extensions
{
    public static IServiceCollection AddRedis(
        this IServiceCollection services , string? connectionString = null , string? prefix= null)
    {
        var serviceProvider = services.BuildServiceProvider().CreateScope().ServiceProvider;
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        
        services.AddSingleton<IRedisClient>(new RedisClient(connectionString??configuration["Redis:ConnectionString"],prefix ?? configuration["Redis:Prefix"]));
        services.AddSingleton(typeof(IRedisCached<>),typeof(RedisCached<>));
        
        return services;
    }
}