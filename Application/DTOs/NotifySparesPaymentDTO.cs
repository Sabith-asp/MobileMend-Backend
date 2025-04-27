using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class NotifySparesPaymentDTO
    {
        public decimal SparesTotal { get; set; }
        public Guid BookingId { get; set; }
        public IEnumerable<GetSpareDTO> Spares { get; set; }

    }
}
