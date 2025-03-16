using TaskManagement.Application.Common.Interfaces;

namespace TaskManagement.Application.Tasks.Commands.UpdateTask;
public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTaskCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0");

        RuleFor(v => v.Title)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueTitle)
                .WithMessage("'{PropertyName}' must be unique.")
                .WithErrorCode("Unique");

        RuleFor(v => v.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

    }
    public async Task<bool> BeUniqueTitle(UpdateTaskCommand model, string title, CancellationToken cancellationToken)
    {
        return !await _context.Tasks
            .Where(l => l.Id != model.Id)
            .AnyAsync(l => l.Title == title, cancellationToken);
    }
}


