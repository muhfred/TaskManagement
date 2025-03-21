﻿using System.Reflection;
using TaskManagement.Application.Tasks.Hubs;
using TaskManagement.Web.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection;

public static class WebApplicationExtensions
{
    public static RouteGroupBuilder MapGroup(this WebApplication app, EndpointGroupBase group)
    {
        var groupName = group.GetType().Name;

        return app
            .MapGroup($"/api/{groupName}");
    }

    public static WebApplication MapEndpoints(this WebApplication app)
    {
        var endpointGroupType = typeof(EndpointGroupBase);

        var assembly = Assembly.GetExecutingAssembly();

        var endpointGroupTypes = assembly.GetExportedTypes()
            .Where(t => t.IsSubclassOf(endpointGroupType));

        foreach (var type in endpointGroupTypes)
        {
            if (Activator.CreateInstance(type) is EndpointGroupBase instance)
            {
                instance.Map(app);
            }
        }

        app.UseCors("CorsPolicy");

        app.MapHub<TaskNotificationHub>("/taskhub");

        app.UseExceptionHandler();

        app.MapRazorPages();

        return app;
    }
}
