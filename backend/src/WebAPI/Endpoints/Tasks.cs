using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using TaskManagement.Application.Common.Models;
using TaskManagement.Application.Tasks.Commands.CreateTask;
using TaskManagement.Application.Tasks.Commands.DeleteTask;
using TaskManagement.Application.Tasks.Commands.UpdateTask;
using TaskManagement.Application.Tasks.Queries.GetTasks;
using TaskManagement.Application.Tasks.Queries.GetTasksWithPagination;
using TaskManagement.Web.Infrastructure;

namespace TaskManagement.WebAPI.Endpoints;

public class Tasks : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetTasksWithPagination)
            .MapPost(CreateTask)
            .MapPut(UpdateTask, "{id}")
            .MapDelete(DeleteTask, "{id}");
    }
    public async Task<Ok<PaginatedList<TaskDto>>> GetTasksWithPagination(ISender sender, [AsParameters] GetTasksWithPaginationQuery query)
    {
        var result = await sender.Send(query);

        return TypedResults.Ok(result);
    }


    public async Task<Created<int>> CreateTask(ISender sender, CreateTaskCommand command)
    {
        var id = await sender.Send(command);

        return TypedResults.Created($"/{nameof(Tasks)}/{id}", id);
    }

    public async Task<Results<NoContent, BadRequest>> UpdateTask(ISender sender, int id, UpdateTaskCommand command)
    {
        if (id != command.Id) return TypedResults.BadRequest();

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    public async Task<NoContent> DeleteTask(ISender sender, int id)
    {
        await sender.Send(new DeleteTaskCommand(id));

        return TypedResults.NoContent();
    }

}
