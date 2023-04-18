namespace DDD.Shop.Infrastructure.DataAccess.Redis;

public class RedisCached<T> : IRedisCached<T>
{
    public T Value { get; set; }

    private readonly IRedisClient _redisClient;

    public RedisCached(IRedisClient redisClient)
    {
        _redisClient = redisClient;
        
        Value = _redisClient.Get<T>(typeof(T).Name) ?? Activator.CreateInstance<T>();
    }


    public void Save(TimeSpan? timeSpan)
    {
        _redisClient.Save(typeof(T).Name,Value,timeSpan);
    }
}