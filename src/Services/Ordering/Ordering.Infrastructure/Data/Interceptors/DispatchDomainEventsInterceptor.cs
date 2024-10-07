using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Ordering.Domain.Abstractions;

namespace Ordering.Infrastructure.Data.Interceptors
{
    public class DispatchDomainEventsInterceptor(IPublisher mediator) : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
            return base.SavingChanges(eventData, result);
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            await DispatchDomainEvents(eventData.Context);
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private async Task DispatchDomainEvents(DbContext? eventDataContext)
        {
            if (eventDataContext == null) return;

            var aggregates = eventDataContext.ChangeTracker.Entries<IAggregate>()
                .Where(s => s.Entity.DomainEvents.Any()).Select(s => s.Entity);

            var domainEvents = aggregates.SelectMany(s => s.DomainEvents).ToList();
            aggregates.ToList().ForEach(s=>s.ClearDomainEvents());

            foreach (var domainEvent in domainEvents) await mediator.Publish(domainEvent);
        }
    }
}
