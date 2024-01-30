using MediatR;
using Microsoft.EntityFrameworkCore;
using NerdStore.Core.Data;
using NerdStore.Core.Messages;
using NerdStore.Sales.Data.Extensions;
using NerdStore.Sales.Domain.Models;

namespace NerdStore.Sales.Data;

public class SalesContext : DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;

    public SalesContext(DbContextOptions<SalesContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Voucher> Vouchers { get; set; }

    public async Task<bool> Commit()
    {
        const string RegistrationDateProperty = "RegistrationDate";

        ChangeTracker
            .Entries()
            .Where(entry => entry.Entity.GetType().GetProperty(RegistrationDateProperty) != null)
            .ToList()
            .ForEach(entry =>
            {
                if (entry.State == EntityState.Added)
                    entry.Property(RegistrationDateProperty).CurrentValue = DateTime.Now;

                if (entry.State == EntityState.Modified)
                    entry.Property(RegistrationDateProperty).IsModified = false;
            });

        var result = await base.SaveChangesAsync() > 0;

        if (result) await _mediator.PublishEventsAsync(this);

        return result;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Model
            .GetEntityTypes()
            .SelectMany(entityType => entityType.GetProperties().Where(p => p.ClrType == typeof(string)))
            .ToList()
            .ForEach(property => property.SetColumnType("varchar(100)"));

        modelBuilder.Ignore<Event>();

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SalesContext).Assembly);

        modelBuilder.Model
            .GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys())
            .ToList()
            .ForEach(relationship => relationship.DeleteBehavior = DeleteBehavior.ClientSetNull);

        modelBuilder.HasSequence<int>("MySequence").StartsAt(1000).IncrementsBy(1);
        base.OnModelCreating(modelBuilder);
    }
}
