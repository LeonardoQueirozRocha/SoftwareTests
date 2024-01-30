using Microsoft.EntityFrameworkCore;
using NerdStore.Catalog.Domain.Models;
using NerdStore.Core.Data;
using NerdStore.Core.Messages;

namespace NerdStore.Catalog.Data;
public class CatalogContext : DbContext, IUnitOfWork
{
    public CatalogContext(DbContextOptions<CatalogContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    public async Task<bool> Commit()
    {
        const string RegistrationDateProperty = "RegistrationDate";

        ChangeTracker
            .Entries()
            .Where(entry => entry.Entity.GetType().GetProperty(RegistrationDateProperty) != null)
            .ToList()
            .ForEach(entry =>
            {
                if (entry.State is EntityState.Added)
                    entry.Property(RegistrationDateProperty).CurrentValue = DateTime.Now;

                if (entry.State is EntityState.Modified)
                    entry.Property(RegistrationDateProperty).IsModified = false;
            });

        return await base.SaveChangesAsync() > 0;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Model
            .GetEntityTypes()
            .SelectMany(entityType => entityType
                .GetProperties()
                .Where(p => p.ClrType == typeof(string)))
            .ToList()
            .ForEach(property => property.SetColumnType("varchar(100)"));

        modelBuilder.Ignore<Event>();

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogContext).Assembly);
    }
}
