using CSSEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSEntity
{
    public class LoginReport : Entity
    {
        public int Id { get; set; }
        //[ForeignKey("EmployeeId")]
        public string EmployeeId { get; set; }
        public DateTime LoginTime { get; set; }
        
        [ForeignKey("EmployeeId")]
        public Employee employee { get; set; }
    }
}
