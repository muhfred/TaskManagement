using Microsoft.Extensions.Logging;
using TaskManagement.Domain.Events;

namespace TaskManagement.Application.Tasks.EventHandlers;


public class TaskCompletedEventHandler : INotificationHandler<TaskCompletedEvent>
{
    private readonly ILogger<TaskCompletedEventHandler> _logger;

    public TaskCompletedEventHandler(ILogger<TaskCompletedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(TaskCompletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("TaskManagement Domain Event: {DomainEvent}", notification.GetType().Name);

        // TODO: Send Notification task completed notification to the user, integrate with other systems, etc.

        return Task.CompletedTask;
    }
}

