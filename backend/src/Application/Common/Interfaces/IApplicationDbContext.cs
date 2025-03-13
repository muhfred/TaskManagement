using Microsoft.EntityFrameworkCore;

namespace TaskManagement.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Domain.Entities.Task> Tasks { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

