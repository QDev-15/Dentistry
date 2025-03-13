using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.GeneratorDB.Entities
{
    public class VisitorLog
    {
        public int Id { set; get; }
        public string IpAddress { get; set; }
        public DateTime VisitTime { get; set; }
    }

}
