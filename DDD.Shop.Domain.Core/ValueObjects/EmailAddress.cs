using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using DDD.Shop.Domain.Core.Serializer.Mongo;
using MongoDB.Bson.Serialization.Attributes;

namespace DDD.Shop.Domain.Core.ValueObjects;

[BsonSerializer(typeof(MongoEmailSerializer))]
public class EmailAddress : ValueObject<EmailAddress>
{
    public string Value { get; }
    
   
    public EmailAddress(string value)
    {
        Value = value;
    }

    public static EmailAddress Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(value));

        try
        {
            var address = new System.Net.Mail.MailAddress(value);
            return new EmailAddress(address.Address);
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
    
    public static implicit operator string(EmailAddress address)
    {
        return address.Value;
    }

    public static implicit operator EmailAddress(string email)
    {
        return new EmailAddress(email);
    }
}
