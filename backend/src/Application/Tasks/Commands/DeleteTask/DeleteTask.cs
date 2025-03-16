using Ardalis.GuardClauses;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Application.Common.Security;

namespace TaskManagement.Application.Tasks.Commands.DeleteTask;

[Authorize]
public record DeleteTaskCommand(int Id) : IRequest;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteTaskCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Tasks
            .FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _context.Tasks.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }

}
