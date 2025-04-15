using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface INotificationService
    {
        Task NotifyTechnician(string technicianId,string message);
        Task NotifyCustomer(string technicianId, string message);
    }
}
