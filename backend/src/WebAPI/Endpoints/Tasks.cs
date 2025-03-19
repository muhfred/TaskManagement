using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using TaskManagement.Application.Tasks.Commands.CreateTask;
using TaskManagement.Application.Tasks.Commands.DeleteTask;
using TaskManagement.Application.Tasks.Commands.UpdateTask;
using TaskManagement.Application.Tasks.Queries.GetTasks;
using TaskManagement.Web.Infrastructure;

namespace TaskManagement.WebAPI.Endpoints;

public class Tasks : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetTasks)
            .MapPost(CreateTask)
            .MapPut(UpdateTask, "{id}")
            .MapDelete(DeleteTask, "{id}");
    }
    public async Task<Ok<List<TaskDto>>> GetTasks(ISender sender)
    {
        var result = await sender.Send(new GetTasksQuery());

        return TypedResults.Ok(result);
    }


    public async Task<Ok<TaskDto>> CreateTask(ISender sender, CreateTaskCommand command)
    {
        var task = await sender.Send(command);

        return TypedResults.Ok(task);
    }

    public async Task<Ok<TaskDto>> UpdateTask(ISender sender, int id)
    {

        var task = await sender.Send(new UpdateTaskCommand(id));

        return TypedResults.Ok(task);
    }

    public async Task<NoContent> DeleteTask(ISender sender, int id)
    {
        await sender.Send(new DeleteTaskCommand(id));

        return TypedResults.NoContent();
    }

}
