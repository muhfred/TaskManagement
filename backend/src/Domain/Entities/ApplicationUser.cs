using Microsoft.AspNetCore.Identity;

namespace TaskManagement.Infrastructure.Data.Identity;
public class ApplicationUser : IdentityUser
{
    public IList<Domain.Entities.Task> Tasks { get; set; } = new List<Domain.Entities.Task>();
}
