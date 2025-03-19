using Ardalis.GuardClauses;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Application.Common.Security;
using TaskManagement.Application.Tasks.Queries.GetTasks;
using TaskManagement.Domain.Events;

namespace TaskManagement.Application.Tasks.Commands.UpdateTask;

[Authorize]
public record UpdateTaskCommand(int Id) : IRequest<TaskDto>;
public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, TaskDto>
{
    private readonly IApplicationDbContext _context;
    public UpdateTaskCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<TaskDto> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Tasks
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.IsCompleted = true;

        entity.AddDomainEvent(new TaskCompletedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return new TaskDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            IsCompleted = entity.IsCompleted
        };
    }
}

