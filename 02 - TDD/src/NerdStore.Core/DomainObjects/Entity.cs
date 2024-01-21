using NerdStore.Core.Messages;

namespace NerdStore.Core.DomainObjects;

public abstract class Entity
{
    private List<Event> _notification;

    public Guid Id { get; set; }
    public IReadOnlyCollection<Event> Notifications => _notification?.AsReadOnly();

    protected Entity()
    {
        Id = Guid.NewGuid();
    }

    public void AddEvent(Event eventItem)
    {
        _notification ??= new List<Event>();
        _notification.Add(eventItem);
    }

    public void RemoveEvent(Event eventItem)
    {
        _notification?.Remove(eventItem);
    }

    public void CleanEvents()
    {
        _notification?.Clear();
    }
}