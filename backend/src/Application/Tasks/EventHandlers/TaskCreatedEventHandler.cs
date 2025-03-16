using Microsoft.Extensions.Logging;
using TaskManagement.Domain.Events;

namespace TaskManagement.Application.Tasks.EventHandlers
{
    public class TaskCreatedEventHandler : INotificationHandler<TaskCreatedEvent>
    {
        private readonly ILogger<TaskCreatedEventHandler> _logger;


        public TaskCreatedEventHandler(ILogger<TaskCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(TaskCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("TaskManagement Domain Event: {DomainEvent}", notification.GetType().Name);

            // TODO: Send task created notifications.

            return Task.CompletedTask;
        }
    }
}
