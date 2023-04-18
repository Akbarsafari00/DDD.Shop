using StackExchange.Redis;

namespace DDD.Shop.Infrastructure.DataAccess.Redis;

public class RedisClient : IDisposable,IRedisClient
{
    private readonly string? _prefix;
    private readonly ConnectionMultiplexer _connection;

    public RedisClient(string? connectionString , string? prefix)
    {
        _prefix = prefix;
        _connection = ConnectionMultiplexer.Connect(connectionString ?? "localhost");
    }

    public IDatabase GetDatabase(int db = -1, object asyncState = null)
    {
        return _connection.GetDatabase(db, asyncState);
    }

    public void Dispose()
    {
        _connection.Dispose();
    }

    public void Save<T>(string key, T value, TimeSpan? expiry = null)
    {
        if (!string.IsNullOrWhiteSpace(_prefix))
            key = $"{_prefix}:{key}";
        
        var db = GetDatabase();
        var serializedValue = Newtonsoft.Json.JsonConvert.SerializeObject(value);
        db.StringSet(key, serializedValue, expiry);
    }

    public T? Get<T>(string key)
    {
        if (!string.IsNullOrWhiteSpace(_prefix))
            key = $"{_prefix}:{key}";
        var db = GetDatabase();
        var serializedValue = db.StringGet(key);
        if (serializedValue.HasValue)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(serializedValue);    
        }
        else
        {
            return default;
        }
        
    }
}