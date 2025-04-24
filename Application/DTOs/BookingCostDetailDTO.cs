using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class BookingCostDetailDTO
    {
        public decimal BookingCharge { get; set; }

        public decimal TravelAllowance { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal TotalBookingCost { get; set; }
    }
}
