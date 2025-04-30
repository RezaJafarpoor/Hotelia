namespace Hotelia.Shared.DDD;

public interface IAggregate : IEntity
{
    List<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}

public interface IAggregate<T> : IAggregate, IEntity<T>
{
    
}