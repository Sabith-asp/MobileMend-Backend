using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class RatingCreateDTO
    {
        public Guid BookingID { get; set; }

        public Guid TechnicianID { get; set; }
        public int RatingNo { get; set; }
        public string ReviewText { get; set; }
    }
}
