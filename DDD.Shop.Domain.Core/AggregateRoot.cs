namespace DDD.Shop.Domain.Core;

public abstract class AggregateRoot<TId> : Entity
{
    
    


    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        // _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        // _domainEvents.Clear();
    }
}
