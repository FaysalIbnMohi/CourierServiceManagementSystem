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
    public class Product:Entity
    {
        public string ProductId { get; set; }
        [Required]
        public string OfficeIdFrom { get; set; }
        [Required]
        public string OfficeIdTo { get; set; }
        [Required]
        public string TripId { get; set; }
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public string ProductType { get; set; }
        [Required]
        public int ProductQuantity { get; set; }
        [Required]
        public int ProductWeight { get; set; }
        [Required]
        public int SendingCost { get; set; }
        [Required]
        public int GivenMoney { get; set; }
        [Required]
        public int ReturnMoney { get; set; }
        [Required]
        public string OderTakenEmployeeId { get; set; }
        [Required]
        public DateTime OderDate { get; set; }
        [Required]
        public string DeliveryStatus { get; set; }

        [ForeignKey("TripId")]
        public Trip Trips { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customers { get; set; }
    }
}
