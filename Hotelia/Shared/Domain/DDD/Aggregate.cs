namespace Hotelia.Shared.Domain.DDD;

public class Aggregate<TId> : IAggregate<TId>, IEntity<TId>
{
    public  TId Id { get; set; } = default!;
    public DateTime? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public List<IDomainEvent> DomainEvents { get; } = new();

    public void ClearDomainEvents()
        => DomainEvents.Clear();

    public void AddDomainEvent(IDomainEvent domainEvent)
        => DomainEvents.Add(domainEvent);
}