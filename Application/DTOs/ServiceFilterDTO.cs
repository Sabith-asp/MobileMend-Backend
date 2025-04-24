using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ServiceFilterDTO
    {
        public Guid? ServiceId { get; set; }
        public string? Search { get; set; }
    }

}
