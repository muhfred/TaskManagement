namespace TaskManagement.Domain.Events;

public class TaskUpdatedEvent : BaseEvent
{
    public TaskUpdatedEvent(Entities.Task task)
    {
        Task = task;
    }
    public Entities.Task Task { get; }
}
