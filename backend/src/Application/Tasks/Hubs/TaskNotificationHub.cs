using Microsoft.AspNetCore.SignalR;

namespace TaskManagement.Application.Tasks.Hubs;

public class TaskNotificationHub : Hub
{
    public async Task SendMessageToUser(string userId, string message)
    {
        await Clients.User(userId).SendAsync("ReceiveNotification", message);
        Console.WriteLine($"Message sent to {userId}: {message}");
    }

    public override async Task OnConnectedAsync()
    {
        await Clients.Caller.SendAsync("Connected", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}
