using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class UpdateCurrentLocationDTO
    {
        public Guid TechnicianId { get; set; }  

        public decimal Latitude { get; set; } = 0;

        public decimal Longitude { get; set; } = 0;
    }
}
