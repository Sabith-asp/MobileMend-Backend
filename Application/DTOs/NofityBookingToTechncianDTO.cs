using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.DTOs
{
    public class NofityBookingToTechncianDTO
    {
        public Guid BookingID { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Issue { get; set; }


        public string Street { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }

        public DateTime CreatedAt { get; set; }

        public string DeviceName { get; set; }
        public string DeviceType { get; set; }


        public string ServiceName { get; set; }


        public string TechnicianName { get; set; }
        public string TechnicianPhone { get; set; }


    }
}
