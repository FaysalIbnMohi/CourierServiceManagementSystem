using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace courier_service_system
{
    public class OfficeViewModel
    {
        public string Division { get; set; }
        [Required]
        public string Area { get; set; }
        [Required]
        public string Road { get; set; }
        [Required]
        public string House { get; set; }
        [Required]
        public int OfficialNumber { get; set; }
    }
}