using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace HeartHotel.Hubs
{
    public class SignalRHub : Hub
    {
        private static readonly ConcurrentBag<string> GroupNames = new ConcurrentBag<string>();
        // Event for notifying all clients
        public async Task NotifyAllClients(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        // Event for notifying a specific client
        public async Task NotifyClient(string connectionId, string message)
        {
            await Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
        }

        // Event for notifying a specific group
        public async Task NotifyGroup(string groupName, string message)
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
        }

        // Event for adding a client to a group
        public async Task AddToGroup(string connectionId, string groupName)
        {
            GroupNames.Add(groupName); // Add group name to the collection
            // await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Groups.AddToGroupAsync(connectionId, groupName);
        }

        // Event for removing a client from a group
        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        // Event triggered when a client connects
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            await Clients.Caller.SendAsync("OnConnected", Context.ConnectionId);
        }

        // Event triggered when a client disconnects
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
            await Clients.All.SendAsync("OnDisconnected", Context.ConnectionId);
        }

        public List<string> GetGroupNames()
        {
            return GroupNames.Distinct().ToList(); // Return all unique group names
        }
    }
}