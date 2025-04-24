using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class BookingEstimateDTO
    {
        public double BookingCharge { get; set; }
        public double TravelAllowance { get; set; }
        public double TotalDistance { get; set; }
        public double TotalCost { get; set; }
    }
}
