using System.Text.Json.Serialization;
using DDD.Shop.Domain.Core;
using DDD.Shop.Domain.Core.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DDD.Shop.Domain.Aggregates.OrderAggregate;

public class Order : AggregateRoot<string>
{

    public Order(BusinessId businessId, EmailAddress email)
    {
        BusinessId = businessId;
        Email = email;
        OrderItems ??= new List<OrderItems>();
    }

    public EmailAddress Email { get; set; }
   
    public List<OrderItems> OrderItems { get; set; }

    public void AddItem(string name, Price price)
    {
        OrderItems.Add(new OrderItems(){Name = name , Price = price});
    }
}

public class OrderItems
{
    public string Name { get; set; }
    public Price Price  { get; set; }
}