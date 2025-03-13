using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TaskManagement.Application.Common.Interfaces;

namespace TaskManagement.Infrastructure.Data;
class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Domain.Entities.Task> Tasks => Set<Domain.Entities.Task>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}