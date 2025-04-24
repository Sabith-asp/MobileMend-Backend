using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class SpareCreateDTO
    {
        public string SpareName { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public Guid BookingID { get; set; }
        public Guid TechnicianID { get; set; }
    }
}
