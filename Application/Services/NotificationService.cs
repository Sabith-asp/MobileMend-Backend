using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;
using Common.Hubs;

namespace Application.Services
{
    public class NotificationService: INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task NotifyTechnician(string technicianId, string message) {
            await _hubContext.Clients.User(technicianId).SendAsync("ReceiveNotification", message);
        }
        public async Task NotifyCustomer(string customerId, string message) {
            await _hubContext.Clients.User(customerId).SendAsync("ReceiveNotification", message);
        }
    }
}
