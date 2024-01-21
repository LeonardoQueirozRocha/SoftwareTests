using MediatR;
using NerdStore.Core.DomainObjects;

namespace NerdStore.Sales.Data.Extensions;

public static class MediatorExtension
{
    public static async Task PublishEventsAsync(this IMediator mediator, SalesContext context)
    {
        var domainEntities = context.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.Notifications != null && x.Entity.Notifications.Any())
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.Notifications)
            .ToList();

        domainEntities.ForEach(entity => entity.Entity.CleanEvents());

        var tasks = domainEvents.Select(async (domainEvent) =>
        {
            await mediator.Publish(domainEvent);
        });

        await Task.WhenAll(tasks);
    }
}