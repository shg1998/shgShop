using System.Runtime.CompilerServices;

namespace Ordering.Domain.Abstractions
{
    public abstract class Aggregate<TId>: Entity<TId>, IAggregate<TId>
    {
        private readonly List<IDomainEvent> _domainEvents = [];
        public IReadOnlyList<IDomainEvent> DomainEvents => this._domainEvents.AsReadOnly();

        public void AddDomainEvent(IDomainEvent domainEvent) => this._domainEvents.Add(domainEvent);

        public IDomainEvent[] ClearDomainEvents()
        {
            var dequeuedEvents = this._domainEvents.ToArray();
            this._domainEvents.Clear();
            return dequeuedEvents;
        }
    }
}
