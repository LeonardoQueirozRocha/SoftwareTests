using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NerdStore.Core.Messages.CommandMessages.Notifications;

namespace NerdStore.Core.Configurations;

public static class CoreDependencyInjectionConfiguration
{
    public static void AddCoreDependencies(this IServiceCollection services)
    {
        services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
    }
}