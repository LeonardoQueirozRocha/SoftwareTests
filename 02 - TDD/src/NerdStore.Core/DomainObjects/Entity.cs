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

    public override bool Equals(object obj)
    {
        var compareTo = obj as Entity;

        if (ReferenceEquals(this, compareTo)) return true;
        if (ReferenceEquals(null, compareTo)) return false;

        return Id.Equals(compareTo.Id);
    }

    public static bool operator ==(Entity a, Entity b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            return true;

        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity a, Entity b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return (GetType().GetHashCode() * 907) + Id.GetHashCode();
    }

    public override string ToString()
    {
        return $"{GetType().Name} [Id={Id}]";
    }

    public virtual bool IsValid()
    {
        throw new NotImplementedException();
    }
}