using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BookingPricing
    {
        public Guid BookingPricingID { get; set; }
        public Guid BookingID { get; set; }
        public double BookingCharge { get; set; }
        public double TravelAllowance { get; set; }
        public double ServiceCharge { get; set; }
        public double TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public double DistanceInKm { get; set; }
    }
}
