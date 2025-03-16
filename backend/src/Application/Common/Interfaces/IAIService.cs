using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Common.Interfaces;

public interface IAIService
{
    public Task<Priority> AnalyzeTaskPriorityAsync(string description);
}
