using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Application.Common.Security;

namespace TaskManagement.Application.Tasks.Queries.GetTasks;

[Authorize]
public record GetTasksQuery : IRequest<List<TaskDto>>;

public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, List<TaskDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTasksQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<TaskDto>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _context.Tasks.OrderByDescending(x => x.CreatedAt).ToListAsync(cancellationToken);
        return _mapper.Map<List<TaskDto>>(tasks);
    }
}
