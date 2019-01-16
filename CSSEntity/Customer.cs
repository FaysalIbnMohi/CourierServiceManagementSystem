using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSSEntity
{
    public class Customer : Entity
    {
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public string ProductId { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public int CustomerPhoneNumber { get; set; }
        [Required]
        public string CustomerEmail { get; set; }
        [Required]
        public string CustomerAddress { get; set; }
        [Required]
        public string ReceiverName { get; set; }
        [Required]
        public int ReceiverPhoneNumber { get; set; }
        [Required]
        public string ReceiverEmail { get; set; }
        [Required]
        public string ReceiverAddress { get; set; }
        [NotMapped]
        public string ReceiverDivision { get; set; }
        [NotMapped]
        public string RceiverArea { get; set; }
        [NotMapped]
        public string ReceiverHouse { get; set; }
    }
}
