using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Application.Common.Security;
using TaskManagement.Application.Tasks.Queries.GetTasks;
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
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var description = request.Description ?? string.Empty;

            var result = await _aIService.AnalyzeTaskPriorityAsync(description);

            var entity = new Domain.Entities.Task
            {
                Title = request.Title,
                Description = request.Description,
                IsCompleted = false,
                Priority = result
            };

            _context.Tasks.Add(entity);

            await transaction.CreateSavepointAsync("BeforeSaving", cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            entity.AddDomainEvent(new TaskCreatedEvent(entity));

            await transaction.CommitAsync(cancellationToken);

            return new TaskDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                IsCompleted = entity.IsCompleted,
                Priority = Enum.GetName(entity.Priority)
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackToSavepointAsync("BeforeSaving", cancellationToken);
            Console.WriteLine(ex.Message);
            return new TaskDto();
        }
    }
}
