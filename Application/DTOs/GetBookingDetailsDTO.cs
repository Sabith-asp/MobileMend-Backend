using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.DTOs
{
    public class GetBookingDetailsDTO
    {
      
            public Guid BookingID { get; set; }
        public Guid TechnicianID { get; set; }
        public string CustomerName { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Issue { get; set; }
            public string BookingStatus { get; set; }
            public string PaymentStatus { get; set; }
            public DateTime CreatedAt { get; set; }

            public int CutomerRating { get; set; }

            public string Street { get; set; }
            public string City { get; set; }
            public string Pincode { get; set; }


            public string DeviceName { get; set; }
            public string DeviceType { get; set; }


            public string ServiceName { get; set; }


            public string TechnicianName { get; set; }
            public string TechnicianPhone { get; set; }

        public IEnumerable<GetSpareDTO> Spares { get; set; }
        public decimal SparesTotal { get; set; }
        public BookingPricing BookingCharge { get; set; }

        public BookingCostDetailDTO bookingCostDetails { get; set; }

    }
}
