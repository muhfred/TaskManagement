using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Application.Common.Security;

namespace TaskManagement.Application.Tasks.Queries.GetTasks;

[Authorize]
public record GetTasksQuery : IRequest<TaskViewModel>;
public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, TaskViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetTasksQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<TaskViewModel> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        return new TaskViewModel
        {
            Tasks = await _context.Tasks
                .AsNoTracking()
                .ProjectTo<TaskDto>(_mapper.ConfigurationProvider)
                .OrderBy(t => t.Title)
                .ToListAsync(cancellationToken)
        };
    }
}

