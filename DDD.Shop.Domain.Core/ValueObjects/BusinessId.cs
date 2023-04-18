using System.Text.Json.Serialization;
using DDD.Shop.Domain.Core.Serializer.Mongo;
using MongoDB.Bson.Serialization.Attributes;

namespace DDD.Shop.Domain.Core.ValueObjects;

[BsonSerializer(typeof(MongoBusinessIdSerializer))]
public class BusinessId : ValueObject<BusinessId>
{
    public Guid Id { get; set; }

  
    public BusinessId(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("The ID cannot be empty.", nameof(id));

        Id = id;
    }

    public static BusinessId NewId()
    {
        return new BusinessId(Guid.NewGuid());
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id;
    }

    public override string ToString()
    {
        return Id.ToString();
    }
    
    public static implicit operator Guid(BusinessId id)
    {
        return id.Id;
    }
    
    public static implicit operator string(BusinessId id)
    {
        return id.Id.ToString();
    }

    public static implicit operator BusinessId(Guid id)
    {
        return new BusinessId(id);
    }
    
   
}
