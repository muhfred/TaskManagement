using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using TaskManagement.Application.Tasks.Hubs;
using TaskManagement.Domain.Events;

namespace TaskManagement.Application.Tasks.EventHandlers;

public class TaskUpdatedEventHandler : INotificationHandler<TaskUpdatedEvent>
{
    private readonly ILogger<TaskCreatedEventHandler> _logger;
    private readonly IHubContext<TaskNotificationHub> _hubContext;
    public TaskUpdatedEventHandler(ILogger<TaskCreatedEventHandler> logger, IHubContext<TaskNotificationHub> hubContext)
    {
        _logger = logger;
        _hubContext = hubContext;
    }
    public Task Handle(TaskUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("TaskManagement Domain Event: {DomainEvent}", notification.GetType().Name);
        _hubContext.Clients.All.SendAsync("TaskPriorityUpdated", notification);

        return Task.CompletedTask;
    }
}
