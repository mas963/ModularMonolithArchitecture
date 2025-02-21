using Microsoft.EntityFrameworkCore;
using ModularMonolith.Shared.Abstractions.Domain;

namespace ModularMonolith.Shared.Infrastructure.Database;

public abstract class DbContextBase : DbContext
{
    protected DbContextBase(DbContextOptions options) : base(options)
    {
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entrties = ChangeTracker
            .Entries()
            .Where(e => e.Entity is AggregateRoot && (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entrties)
        {
            var entity = (AggregateRoot)entry.Entity;

            if (entry.State == EntityState.Added)
            {
                entity.SetCreatedDate(DateTime.UtcNow);
            }

            entity.SetModifiedDate(DateTime.UtcNow);
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
