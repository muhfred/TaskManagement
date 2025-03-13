namespace TaskManagement.Domain.Events;

public class TaskCreatedEvent : BaseEvent
{
    public TaskCreatedEvent(Entities.Task task)
    {
        Task = task;
    }

    public Entities.Task Task { get; }
}

