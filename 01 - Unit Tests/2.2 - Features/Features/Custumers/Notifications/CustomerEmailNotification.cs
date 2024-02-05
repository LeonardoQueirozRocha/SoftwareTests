using MediatR;

namespace Features.Custumers.Notifications;

public class CustomerEmailNotification : INotification
{
    public string Origin { get; private set; }
    public string Destiny { get; private set; }
    public string Subject { get; private set; }
    public string Message { get; private set; }

    public CustomerEmailNotification(
        string origin,
        string destiny,
        string subject,
        string message)
    {
        Origin = origin;
        Destiny = destiny;
        Subject = subject;
        Message = message;
    }
}