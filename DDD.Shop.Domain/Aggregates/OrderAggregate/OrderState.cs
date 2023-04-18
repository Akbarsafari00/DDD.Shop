using DDD.Shop.Domain.Core;

namespace DDD.Shop.Domain.Aggregates.OrderAggregate;

public class OrderStatus : Enumeration<OrderStatus>
{
    public static readonly OrderStatus Pending = new OrderStatus(nameof(Pending), 1);
    public static readonly OrderStatus Processing = new OrderStatus(nameof(Processing), 2);
    public static readonly OrderStatus Shipped = new OrderStatus(nameof(Shipped), 3);

    private OrderStatus(string name, int value) : base(name, value)
    {
    }
}
