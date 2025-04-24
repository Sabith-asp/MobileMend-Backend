using Microsoft.AspNetCore.SignalR;

namespace Common.Hubs
{
    public class NotificationHub : Hub
    {
        public static readonly Dictionary<string, string> _connections = new Dictionary<string, string>();

        // Store the technician connection when they connect
        public override async Task OnConnectedAsync()
        {
            var technicianId = Context.GetHttpContext()?.Request?.Query["technicianId"]; // Get technicianId from query parameters

            // Check if technicianId has a value
            if (!string.IsNullOrEmpty(technicianId))
            {
                _connections[technicianId] = Context.ConnectionId; // Map technicianId to connectionId
                Console.WriteLine($"Technician connected: {technicianId}");
            }

            await base.OnConnectedAsync();
        }

        // Handle disconnection of the technician
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var technicianId = _connections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
            if (technicianId != null)
            {
                _connections.Remove(technicianId); // Remove the technician from the connection dictionary
                Console.WriteLine($"Technician disconnected: {technicianId}");
            }

            await base.OnDisconnectedAsync(exception);
        }

        // Method to send a notification to a specific technician
        public async Task SendNotificationToTechnician(string technicianId, string message)
        {
            if (_connections.TryGetValue(technicianId, out var connectionId))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveNotification", message);
            }
        }
    }
}
