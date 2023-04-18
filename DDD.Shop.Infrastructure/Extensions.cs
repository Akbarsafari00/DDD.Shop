using DDD.Shop.Domain.Core;
using DDD.Shop.Infrastructure.DataAccess.Mongo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace DDD.Shop.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services )
    {
        var serviceProvider = services.BuildServiceProvider().CreateScope().ServiceProvider;
        return services;
    }
}