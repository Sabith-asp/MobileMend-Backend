using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class BookingReassignmentDataDTO
    {
        public Guid DeviceId { get; set; }
        public Guid AddressId { get; set; }
    }
}
