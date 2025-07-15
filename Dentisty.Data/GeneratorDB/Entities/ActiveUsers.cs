using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.GeneratorDB.Entities
{
    public class ActiveUser
    {
        public int Id { set; get; }
        public string IpAddress { get; set; }
        public DateTime LastActive { get; set; }
        public string? VisitorId { get; set; }
        public string? UserAgent { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public bool IsOnline { get; set; } = false;
    }
}
