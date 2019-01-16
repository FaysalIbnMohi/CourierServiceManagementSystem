using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CSSEntity
{
    public class MailModel : Entity
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        [MaxLength(1000)]
        public string Body { get; set; }
        public DateTime Date { get; set; }
    }
}