using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Spare
    {
        public Guid ID { get; set; }
        public string SpareName { get; set; }   
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime AddedAt { get; set; }
        public Guid AddedBy { get; set; }
        public Guid BookingID { get; set; }

    }
}
