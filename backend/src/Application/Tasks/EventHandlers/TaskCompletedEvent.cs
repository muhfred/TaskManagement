﻿using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using TaskManagement.Application.Tasks.Hubs;
using TaskManagement.Domain.Events;

namespace TaskManagement.Application.Tasks.EventHandlers;


public class TaskCompletedEventHandler : INotificationHandler<TaskCompletedEvent>
{
    private readonly ILogger<TaskCompletedEventHandler> _logger;
    private readonly IHubContext<TaskNotificationHub> _hubContext;
    public TaskCompletedEventHandler(ILogger<TaskCompletedEventHandler> logger, IHubContext<TaskNotificationHub> hubContext)
    {
        _logger = logger;
        _hubContext = hubContext;
    }

    public Task Handle(TaskCompletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("TaskManagement Domain Event: {DomainEvent}", notification.GetType().Name);
        _hubContext.Clients.All.SendAsync("TaskCompleted", notification);
        return Task.CompletedTask;
    }
}

