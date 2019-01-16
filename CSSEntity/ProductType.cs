using CSSEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSEntity
{
   public class ProductType:Entity
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int ShipmentCost { get; set; }
        [Required]
        public int Vat { get; set; }
    }
}
