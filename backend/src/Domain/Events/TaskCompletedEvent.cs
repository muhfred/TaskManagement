namespace TaskManagement.Domain.Events;
public class TaskCompletedEvent : BaseEvent
{
    public TaskCompletedEvent(Entities.Task task)
    {
        Task = task;
    }

    public Entities.Task Task { get; }
}
