using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Application.Common.Security;
using TaskManagement.Application.Tasks.Queries.GetTasks;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Events;

namespace TaskManagement.Application.Tasks.Commands.CreateTask;

[Authorize]
public record CreateTaskCommand(string Title, string Description) : IRequest<TaskDto>;
public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto>
{
    private readonly IApplicationDbContext _context;

    private readonly IAIService _aIService;
    private readonly IUser _currentUser;
    private readonly IMapper _mapper;
    public CreateTaskCommandHandler(IApplicationDbContext context, IAIService aIService, IUser currentUser, IMapper mapper)
    {
        _context = context;
        _aIService = aIService;
        _currentUser = currentUser;
        _mapper = mapper;
    }
    public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.Task
        {
            Title = request.Title,
            Description = request.Description,
            IdentityUserId = _currentUser.Id,
            IsCompleted = false
        };

        _context.Tasks.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        entity.AddDomainEvent(new TaskCreatedEvent(entity));

        if (!string.IsNullOrEmpty(request.Description))
        {
            try
            {
                var suggestedPriority = await _aIService.AnalyzeTaskPriorityAsync(request.Description).ConfigureAwait(true);
                var taskDto = await UpdateTaskPriorityAsync(entity.Id, suggestedPriority, cancellationToken);
                if (taskDto.Id > 0)
                {
                    entity.AddDomainEvent(new TaskUpdatedEvent(entity));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error analyzing task priority: {ex.Message}");
                // Continue without setting priority
            }
        }

        return new TaskDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            IsCompleted = entity.IsCompleted
        };
    }

    private async Task<TaskDto> UpdateTaskPriorityAsync(int taskId, Priority priority, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks.FindAsync([taskId], cancellationToken);
        if (task == null)
        {
            return new TaskDto();
        }
        task.Priority = priority;
        task.AddDomainEvent(new TaskUpdatedEvent(task));
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<TaskDto>(task);
    }
}
