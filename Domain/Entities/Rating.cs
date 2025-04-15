using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Rating
    {
        public Guid RatingID { get; set; }
        public Guid BookingID { get; set; }
        public Guid TechnicianID { get; set; }
        public int RatingNo { get; set; }
        public string ReviewText { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
