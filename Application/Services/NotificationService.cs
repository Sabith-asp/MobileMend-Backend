using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;
using Common.Hubs;
using Application.DTOs;

namespace Application.Services
{
    public class NotificationService: INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task NotifyTechnician(string technicianId, GetBookingDetailsDTO message)
        {
            if (NotificationHub._connections.TryGetValue(technicianId, out var connectionId))
            {
                await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification", message);
            }
        }


        public async Task NotifyCustomer(string customerId, string message) {
            await _hubContext.Clients.User(customerId).SendAsync("ReceiveNotification", message);
        }
    }
}
