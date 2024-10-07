using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Ordering.Domain.Abstractions;

namespace Ordering.Infrastructure.Data.Interceptors
{
    public class AuditableEntityInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            UpdateEntities(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void UpdateEntities(DbContext? eventDataContext)
        {
            if(eventDataContext == null) return;
            foreach (var entry in eventDataContext.ChangeTracker.Entries<IEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = "shg1998";
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }

                if (entry.State != EntityState.Added && entry.State != EntityState.Modified &&
                    !entry.HasChangedOwnedEntities()) continue;
                entry.Entity.CreatedBy = "shg1998";
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }
        }
    }

    public static class Extensions
    {
        public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
            entry.References.Any(s =>
                s.TargetEntry != null && s.TargetEntry.Metadata.IsOwned() &&
                s.TargetEntry.State is EntityState.Added or EntityState.Modified);
    }
}
