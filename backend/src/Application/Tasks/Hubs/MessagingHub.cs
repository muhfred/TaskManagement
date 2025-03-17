using Microsoft.AspNetCore.SignalR;
namespace TaskManagement.Application.Tasks.Hubs
{
    public class UserMessage
    {
        public required string Sender { get; set; }
        public required string Content { get; set; }
        public DateTime SentTime { get; set; }
    }
    public class MessagingHub : Hub
    {
        private static readonly List<UserMessage> MessageHistory = new List<UserMessage>();
        public async Task PostMessage(string content)
        {
            var senderId = Context.ConnectionId;
            var userMessage = new UserMessage
            {
                Sender = senderId,
                Content = content,
                SentTime = DateTime.UtcNow
            };
            MessageHistory.Add(userMessage);
            await Clients.All.SendAsync("ReceiveMessage", senderId, content, userMessage.SentTime);
        }

        public async Task ReceiveMessage(string content)
        {
            var senderId = Context.ConnectionId;
            var userMessage = new UserMessage
            {
                Sender = senderId,
                Content = content,
                SentTime = DateTime.UtcNow
            };
            MessageHistory.Add(userMessage);
            await Clients.All.SendAsync("ReceiveMessage", senderId, content, userMessage.SentTime);
        }
        public async Task RetrieveMessageHistory() =>
            await Clients.Others.SendAsync("RetrieveMessageHistory", MessageHistory);
    }
}
