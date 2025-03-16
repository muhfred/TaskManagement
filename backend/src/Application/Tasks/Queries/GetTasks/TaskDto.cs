namespace TaskManagement.Application.Tasks.Queries.GetTasks;
public class TaskDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Priority { get; set; }
    public string? IdentityUserId { get; set; }

    public bool IsCompleted { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Task, TaskDto>();
        }
    }
}
