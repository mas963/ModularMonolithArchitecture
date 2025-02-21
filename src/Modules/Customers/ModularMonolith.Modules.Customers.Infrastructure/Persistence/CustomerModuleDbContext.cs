using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ModularMonolith.Modules.Customers.Domain.Entities;
using ModularMonolith.Modules.Customers.Infrastructure.Persistence.Configurations;

namespace ModularMonolith.Modules.Customers.Infrastructure.Persistence;

public class CustomerModuleDbContext : IdentityDbContext<Customer, IdentityRole<Guid>, Guid> 
{
    public CustomerModuleDbContext(DbContextOptions<CustomerModuleDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("customerIdentity");

        builder.ApplyConfiguration(new CustomerConfiguration());

        // Identity tablolarını özelleştirme
        builder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customers");
        });

        builder.Entity<IdentityRole<Guid>>(entity =>
        {
            entity.ToTable("Roles");
        });

        builder.Entity<IdentityUserRole<Guid>>(entity =>
        {
            entity.ToTable("CustomerRoles");
        });

        builder.Entity<IdentityUserClaim<Guid>>(entity =>
        {
            entity.ToTable("CustomerClaims");
        });

        builder.Entity<IdentityUserLogin<Guid>>(entity =>
        {
            entity.ToTable("CustomerLogins");
        });

        builder.Entity<IdentityRoleClaim<Guid>>(entity =>
        {
            entity.ToTable("CustomerRoleClaims");
        });

        builder.Entity<IdentityUserToken<Guid>>(entity =>
        {
            entity.ToTable("CustomerTokens");
        });
    }
}
