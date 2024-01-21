using MediatR;

namespace NerdStore.Core.Messages;

public abstract class Event : Message, INotification
{
    public DateTime Timestamp { get; }

    protected Event()
    {
        Timestamp = DateTime.Now;
    }
}