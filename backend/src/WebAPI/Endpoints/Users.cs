using TaskManagement.Infrastructure.Data.Identity;
using TaskManagement.Web.Infrastructure;

namespace TaskManagement.WebAPI.Endpoints;

public class Users : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
          .MapIdentityApi<ApplicationUser>();
    }
}
