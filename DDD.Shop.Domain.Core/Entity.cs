using System.ComponentModel;
using DDD.Shop.Domain.Core.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DDD.Shop.Domain.Core;

public abstract class Entity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get;  set; }

    public BusinessId BusinessId { get; set; }

    [BsonRepresentation(BsonType.String)]
    public DateTime CreatedAt { get; set; }
    [BsonRepresentation(BsonType.String)]
    public DateTime? UpdatedAt { get; set; }
    [BsonRepresentation(BsonType.String)]
    public DateTime? DeletedAt { get; set; }
    
    [DefaultValue(false)]
    [BsonDefaultValue(false)]
    public bool IsDeleted { get; set; }
    public override bool Equals(object obj)
    {
        if (obj == null || !(obj is string other))
        {
            return false;
        }

        return Id.Equals(other);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(Entity left, Entity right)
    {
        if (ReferenceEquals(left, null))
        {
            return ReferenceEquals(right, null);
        }

        return left.Equals(right);
    }

    public static bool operator !=(Entity left, Entity right)
    {
        return !(left == right);
    }
}