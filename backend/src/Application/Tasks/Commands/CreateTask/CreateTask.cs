using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Application.Common.Security;
using TaskManagement.Domain.Events;

namespace TaskManagement.Application.Tasks.Commands.CreateTask;

[Authorize]
public record CreateTaskCommand(string Title, string Description, int Priority, string IdentityUserId) : IRequest<int>;
public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IAIService _aIService;
    public CreateTaskCommandHandler(IApplicationDbContext context, IAIService aIService)
    {
        _context = context;
        _aIService = aIService;
    }
    public async Task<int> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {

        var description = request.Description ?? string.Empty;

        var result = await _aIService.AnalyzeTaskPriorityAsync(description).ConfigureAwait(true);

        var entity = new Domain.Entities.Task
        {
            Title = request.Title,
            Description = request.Description,
            IdentityUserId = request.IdentityUserId,
            IsCompleted = false,
            Priority = result
        };

        entity.AddDomainEvent(new TaskCreatedEvent(entity));

        _context.Tasks.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
