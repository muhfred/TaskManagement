using TaskManagement.Application.Common.Interfaces;

namespace TaskManagement.Application.Tasks.Commands.CreateTask
{
    class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
    {
        private readonly IApplicationDbContext _context;
        public CreateTaskCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.Title)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueTitle)
                .WithMessage("'{PropertyName}' must be unique.")
                .WithErrorCode("Unique");

            RuleFor(v => v.Description)
                .MaximumLength(1000)
                .NotEmpty();

        }

        public async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
        {
            return !await _context.Tasks
                .AnyAsync(l => l.Title == title, cancellationToken);
        }
    }
}
