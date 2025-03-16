using TaskManagement.Application.Tasks.Queries.GetTasksWithPagination;

namespace TaskManagement.Application.Tasks.Queries.GetTasks;
public class GetTasksWithPaginationQueryValidator : AbstractValidator<GetTasksWithPaginationQuery>
{
    public GetTasksWithPaginationQueryValidator()
    {

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
    }
}
