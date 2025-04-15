using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.DTOs
{
    public class TechncianRequestAddDTO
    {

        public Guid UserID { get; set; }

        public int Experience { get; set; }

        public string Resume { get; set; }

        public string Specialization { get; set; }

        public string Bio { get; set; }

        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
    }
}
