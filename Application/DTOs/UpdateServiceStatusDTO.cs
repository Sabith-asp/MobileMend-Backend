using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.DTOs
{
    public class UpdateServiceStatusDTO
    {
        public Guid TechnicianId { get; set; }
        public ServiceStatus Status { get; set; }
        public Guid BookingId { get; set; }
    }
}
