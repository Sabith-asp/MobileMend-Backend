using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetSpareDTO
    {
        public Guid ID { get; set; }
        public string SpareName { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public decimal TotalCost { get; set; }

    }
}
