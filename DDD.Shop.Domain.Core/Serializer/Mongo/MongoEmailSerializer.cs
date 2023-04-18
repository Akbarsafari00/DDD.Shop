using DDD.Shop.Domain.Core.ValueObjects;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace DDD.Shop.Domain.Core.Serializer.Mongo;
public class MongoEmailSerializer : SerializerBase<EmailAddress>
{
    public override EmailAddress Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        return context.Reader.ReadString();
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, EmailAddress value)
    {
        context.Writer.WriteString(value);
    }
}