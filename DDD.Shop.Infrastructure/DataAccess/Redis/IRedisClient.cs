namespace DDD.Shop.Infrastructure.DataAccess.Redis;

public interface IRedisClient : IDisposable
{
    void Save<T>(string key, T value, TimeSpan? expiry = null);
    T? Get<T>(string key);
}