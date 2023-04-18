using DDD.Shop.Domain.Core.ValueObjects;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace DDD.Shop.Domain.Core.Serializer.Mongo;

public class MongoPriceSerializer : SerializerBase<Price>
{
    public override Price Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var reader = context.Reader;
        reader.ReadStartDocument();
        var price = reader.ReadDecimal128("Amount");
        var priceCurrency = reader.ReadString("Currency");
        reader.ReadEndDocument();
        return Price.Create(decimal.Parse(price.ToString()), priceCurrency);
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Price value)
    {
        var writer = context.Writer;
        writer.WriteStartDocument();
        writer.WriteDecimal128("Amount", value.Amount);
        writer.WriteString("Currency", value.Currency);
        writer.WriteEndDocument();
    }
}