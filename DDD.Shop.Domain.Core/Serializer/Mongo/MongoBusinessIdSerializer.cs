using DDD.Shop.Domain.Core.ValueObjects;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace DDD.Shop.Domain.Core.Serializer.Mongo;


public class MongoBusinessIdSerializer : SerializerBase<BusinessId>
{
    public override BusinessId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var x = context.Reader.ReadString();

        return Guid.Parse(x);
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, BusinessId value)
    {
        context.Writer.WriteString(value);
    }
}