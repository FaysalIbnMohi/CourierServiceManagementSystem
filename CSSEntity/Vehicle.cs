using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSEntity
{
    public class Vehicle : Entity
    {
        [Required]
        public string VehicleRegisteredByEmployee { get; set; }
        [Required]
        public DateTime VehicleRegisterDate { get; set; }
        [Required,Key]
        public string VehicleNumber { get; set; }
        public string VehicleEntryByEmployeeId { get; set; }
        public string CurrentLocation { get; set; }
        public string TripId { get; set; }

        [ForeignKey("TripId")]
        public Trip Trips { get; set; }
    }
}
