using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using DDD.Shop.Domain.Core.Serializer.Mongo;
using MongoDB.Bson.Serialization.Attributes;

namespace DDD.Shop.Domain.Core.ValueObjects;

[BsonSerializer(typeof(MongoPhoneNumberSerializer))]
public class PhoneNumber : ValueObject<PhoneNumber>
{
    public string Value { get; }
    [JsonConstructor]
    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static PhoneNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(value));

        try
        {
            var address = new string(value.Where(char.IsDigit).ToArray());
            return new PhoneNumber(address);
        }
        catch (FormatException)
        {
            throw new FormatException("Invalid email address format.");
        }
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value.ToLower();
    }

    public override string ToString()
    {
        return Value;
    }
    
    public static implicit operator string(PhoneNumber phone)
    {
        return phone;
    }

    public static implicit operator PhoneNumber(string phone)
    {
        return new PhoneNumber(phone);
    }
}
