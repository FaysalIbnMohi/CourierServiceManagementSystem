using System;

namespace CSSEntity
{
    public class RequestUpdate : Entity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public string NID { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
