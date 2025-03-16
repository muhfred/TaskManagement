using Ardalis.GuardClauses;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Application.Common.Security;
using TaskManagement.Domain.Events;

namespace TaskManagement.Application.Tasks.Commands.UpdateTask;

[Authorize]
public record UpdateTaskCommand(int Id, string Title, string Description, bool IsCompleted) : IRequest;
public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand>
{
    private readonly IApplicationDbContext _context;
    public UpdateTaskCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Tasks
            .FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Title = request.Title;
        entity.Description = request.Description;
        entity.IsCompleted = request.IsCompleted;

        if (entity.IsCompleted)
        {
            entity.AddDomainEvent(new TaskCompletedEvent(entity));
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}

