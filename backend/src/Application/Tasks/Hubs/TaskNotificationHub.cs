using Microsoft.AspNetCore.SignalR;
using TaskManagement.Application.Tasks.Queries.GetTasks;

namespace TaskManagement.Application.Tasks.Hubs;

public class TaskNotificationHub : Hub
{
    //public override async Task OnConnectedAsync()
    //{
    //    await Clients.All.SendAsync("Connected", Context.ConnectionId);
    //    await base.OnConnectedAsync();
    //}

    public async Task NotifyTaskCreated(TaskDto task)
    {
        await Clients.All.SendAsync("TaskCreated", task);
    }

    public async Task NotifyTaskCompleted(TaskDto task)
    {
        await Clients.All.SendAsync("TaskCompleted", task);
    }

    public async Task NotifyTaskPriorityUpdated(TaskDto task)
    {
        await Clients.All.SendAsync("TaskPriorityUpdated", task);
    }

    //public override async Task OnDisconnectedAsync(Exception exception)
    //{
    //    await base.OnDisconnectedAsync(exception);
    //}
}
