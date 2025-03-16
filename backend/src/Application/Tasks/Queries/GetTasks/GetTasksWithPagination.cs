using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Application.Common.Mappings;
using TaskManagement.Application.Common.Models;
using TaskManagement.Application.Tasks.Queries.GetTasks;

namespace TaskManagement.Application.Tasks.Queries.GetTasksWithPagination;

public record GetTasksWithPaginationQuery : IRequest<PaginatedList<TaskDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetTasksWithPaginationQueryHandler : IRequestHandler<GetTasksWithPaginationQuery, PaginatedList<TaskDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTasksWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<TaskDto>> Handle(GetTasksWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Tasks
            .OrderBy(x => x.Title)
            .ProjectTo<TaskDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
