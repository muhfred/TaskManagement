namespace TaskManagement.Domain.Entities;

public class Task : BaseAuditableEntity
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public Priority Priority { get; set; } = Priority.None;
    public string? IdentityUserId { get; set; }

    private bool _isCompleted;

    public bool IsCompleted
    {
        get => _isCompleted;
        set
        {
            if (value && !_isCompleted)
            {
                AddDomainEvent(new Events.TaskCompletedEvent(this));
            }
            _isCompleted = value;
        }
    }
}

