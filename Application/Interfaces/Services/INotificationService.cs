using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;

namespace Application.Interfaces.Services
{
    public interface INotificationService
    {
        Task NotifyTechnician(string technicianId, NofityBookingToTechncianDTO message);
        Task NotifyCustomer(string technicianId, NotifySparesPaymentDTO message);
    }
}
