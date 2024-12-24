using Dentistry.Data.GeneratorDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.GeneratorDB.Entities
{
    public class Branches
    {
        public int Id {set; get;}
        public string Name {set; get;}
        public string? Code {set; get;}
        public string Address {set; get;}
        public string PhoneNumber {set; get;}
        public DateTime CreatedAt {set; get;}
        public DateTime UpdatedAt { set; get; }
        public List<Contact> Contacts {set; get;} = new List<Contact>();
    }
}
