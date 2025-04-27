using Microsoft.AspNetCore.SignalR;

namespace Common.Hubs
{
    public class NotificationHub : Hub
    {
        public static readonly Dictionary<string, string> _connections = new Dictionary<string, string>();

        // Store the technician connection when they connect
        public override async Task OnConnectedAsync()
        {
            var technicianId = Context.GetHttpContext()?.Request?.Query["technicianId"];
            var customerId = Context.GetHttpContext()?.Request?.Query["customerId"];

            if (!string.IsNullOrEmpty(technicianId))
            {
                _connections[technicianId] = Context.ConnectionId;
                Console.WriteLine($"Technician connected: {technicianId}");
            }
            else if (!string.IsNullOrEmpty(customerId))
            {
                _connections[customerId] = Context.ConnectionId;
                Console.WriteLine($"Customer connected: {customerId}");
            }

            await base.OnConnectedAsync();
        }


        // Handle disconnection of the technician
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // Try to find the userId (technician or customer) based on the connection ID
            var userId = _connections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;

            if (userId != null)
            {
                _connections.Remove(userId);  // Remove user from the connections dictionary
                Console.WriteLine($"{userId} disconnected.");
            }
            else
            {
                Console.WriteLine($"Connection ID {Context.ConnectionId} not found.");
            }

            await base.OnDisconnectedAsync(exception);
        }

        // Method to send a notification to a specific technician
        //public async Task SendNotificationToTechnician(string technicianId, string message)
        //{
        //    if (_connections.TryGetValue(technicianId, out var connectionId))
        //    {
        //        await Clients.Client(connectionId).SendAsync("ReceiveNotification", message);
        //    }
        //}
    }
}
