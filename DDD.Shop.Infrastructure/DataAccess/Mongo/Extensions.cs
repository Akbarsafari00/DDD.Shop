using DDD.Shop.Domain.Core;
using DDD.Shop.Domain.Repositories;
using DDD.Shop.Infrastructure.DataAccess.Mongo.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace DDD.Shop.Infrastructure.DataAccess.Mongo;

public static class Extensions
{
    public static IServiceCollection AddMongoDB(this IServiceCollection services ,string? connectionString = null , string? databaseName = null)
    {
        var serviceProvider = services.BuildServiceProvider().CreateScope().ServiceProvider;
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            connectionString = configuration["Mongo:ConnectionString"];
        }
        
        if (string.IsNullOrWhiteSpace(databaseName))
        {
            databaseName = configuration["Mongo:DatabaseName"];
        }
        
        
        var client = new MongoClient(connectionString);

        var database = client.GetDatabase(databaseName);

        services.AddSingleton(database);

        services.AddTransient<IOrderRepository, OrderRepository>();
        
        return services;
    }
}