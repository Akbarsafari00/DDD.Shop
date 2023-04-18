namespace DDD.Shop.Infrastructure.DataAccess.Redis;

public interface IRedisCached<T>
{
    public T Value { get; set; }
    void Save(TimeSpan? timeSpan);
}