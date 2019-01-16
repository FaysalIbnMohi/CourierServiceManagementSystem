using CSSEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSEntity
{
   public class Employee : Entity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PrfoilePicture { get; set; }
        public DateTime Birthday { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public int PhoneNumber { get; set; }
        public string Email { get; set; }
        public string NationalId { get; set; }
        public String OfficeId { get; set; }
        public DateTime JoinDate { get; set; }
        public string Position { get; set; }
        public int Salary { get; set; }
        public DateTime RegistrationTime { get; set; }
        public String CurrentStatus { get; set; }
        public String CurrentLocation { get; set; }

        [ForeignKey("OfficeId")]
        public Office offices { get; set; }
        public Employee()
        {
            this.Birthday = DateTime.Now.Date;
            this.JoinDate = DateTime.Now.Date;
        }
    }
}
