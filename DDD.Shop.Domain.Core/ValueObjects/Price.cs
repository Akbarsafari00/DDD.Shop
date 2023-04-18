using System.Text.Json.Serialization;
using DDD.Shop.Domain.Core.Serializer.Mongo;
using MongoDB.Bson.Serialization.Attributes;

namespace DDD.Shop.Domain.Core.ValueObjects;

[BsonSerializer(typeof(MongoPriceSerializer))]
public class Price : ValueObject<Price>
{
    public decimal Amount { get; }

    public string Currency { get; }

   
 
    public Price(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Price Create(decimal amount, string currencyCode)
    {
        if (string.IsNullOrEmpty(currencyCode))
        {
            throw new ArgumentNullException(nameof(currencyCode));
        }

        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Price cannot be negative.");
        }

        return new Price(amount, currencyCode);
    }


    public override string ToString()
    {
        return $"{Amount} {Currency}";
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
}
