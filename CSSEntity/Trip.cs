using CSSEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSEntity
{
    public class Trip: Entity
    {
        [Required,Key]
        public string TripId { get; set; }
        [Required]
        public DateTime TripDate { get; set; }
        [Required]
        public string StartOfficeId { get; set; }
        [Required]
        public string DestinationOfficeId { get; set; }
        [Required]
        public string Id { get; set; }
        [Required]
        public string VehicleNumber { get; set; }

        [ForeignKey("Id")]
        public Employee Employees { get; set; }

        public Trip()
        {
            TripDate = DateTime.Now.Date;
        }
    }
}
