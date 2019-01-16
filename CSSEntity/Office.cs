using CSSEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSEntity
{
    public class Office: Entity
    {
        [Required]
        public string OfficeId { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public int OfficialNumber { get; set; }
    }
}
