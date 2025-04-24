using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class BookingResultDTO
    {
        public Guid BookingId { get; set; }
        public int RowsAffected { get; set; }
    }
}
