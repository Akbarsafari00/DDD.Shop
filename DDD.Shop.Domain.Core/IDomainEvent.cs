namespace DDD.Shop.Domain.Core;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
