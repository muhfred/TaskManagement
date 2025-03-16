using Microsoft.AspNetCore.SignalR;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Application.Common.Security;
using TaskManagement.Application.Tasks.Hubs;
using TaskManagement.Domain.Events;

namespace TaskManagement.Application.Tasks.Commands.CreateTask;

[Authorize]
public record CreateTaskCommand(string Title, string Description, int Priority) : IRequest<int>;
public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IHubContext<TaskNotificationHub> _hubContext;
    private readonly IAIService _aIService;
    private readonly IUser _currentUser;
    public CreateTaskCommandHandler(IApplicationDbContext context, IAIService aIService, IHubContext<TaskNotificationHub> hubContext, IUser currentUser)
    {
        _context = context;
        _aIService = aIService;
        _hubContext = hubContext;
        _currentUser = currentUser;
    }
    public async Task<int> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var description = request.Description ?? string.Empty;

            var result = await _aIService.AnalyzeTaskPriorityAsync(description).ConfigureAwait(true);

            var entity = new Domain.Entities.Task
            {
                Title = request.Title,
                Description = request.Description,
                IdentityUserId = _currentUser.Id,
                IsCompleted = false,
                Priority = result
            };

            _context.Tasks.Add(entity);

            await transaction.CreateSavepointAsync("BeforeSaving", cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            entity.AddDomainEvent(new TaskCreatedEvent(entity));

            await _hubContext.Clients.User(entity.IdentityUserId).SendAsync("TaskCompleted", entity.Id, cancellationToken: cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return entity.Id;
        }
        catch (Exception ex)
        {
            await transaction.RollbackToSavepointAsync("BeforeSaving", cancellationToken);
            Console.WriteLine(ex.Message);
            return 0;
        }
    }
}
