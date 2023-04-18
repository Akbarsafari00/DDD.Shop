using DDD.Shop.Domain.Core.ValueObjects;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace DDD.Shop.Domain.Core.Serializer.Mongo;


public class MongoPhoneNumberSerializer : SerializerBase<PhoneNumber>
{
    public override PhoneNumber Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        return context.Reader.ReadString();
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, PhoneNumber value)
    {
        context.Writer.WriteString(value);
    }
}