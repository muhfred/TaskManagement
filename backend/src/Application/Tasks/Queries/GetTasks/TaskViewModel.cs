namespace TaskManagement.Application.Tasks.Queries.GetTasks;
public class TaskViewModel
{
    public IReadOnlyCollection<TaskDto> Tasks { get; init; } = [];
}
