using ModularMonolith.Shared.Abstractions.Events;

namespace ModularMonolith.Shared.Abstractions.Domain;

public abstract class AggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public Guid Id { get; protected set; }
    public DateTime CreatedDate { get; protected set; }
    public DateTime? ModifiedDate { get; protected set; }

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public void SetCreatedDate(DateTime date)
    {
        CreatedDate = date;
    }

    public void SetModifiedDate(DateTime date)
    {
        ModifiedDate = date;
    }
}
