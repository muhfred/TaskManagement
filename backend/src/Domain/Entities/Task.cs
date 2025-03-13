namespace TaskManagement.Domain.Entities;

public class Task : BaseAuditableEntity
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public Priority Priority { get; set; } = Priority.None;

    private bool _IsCompleted;

    public bool IsCompleted
    {
        get => _IsCompleted;
        set
        {
            if (value && !_IsCompleted)
            {
                AddDomainEvent(new Events.TaskCompletedEvent(this));
            }
            _IsCompleted = value;
        }
    }
}

