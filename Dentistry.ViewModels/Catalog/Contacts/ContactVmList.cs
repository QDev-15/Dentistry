using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.ViewModels.Catalog.Contacts
{
    public class ContactVmList
    {
        public List<ContactVm> ContactActives { get; set; } = new List<ContactVm>();
        public List<ContactVm> ContactInActives { get; set; } = new List<ContactVm>();
    }
}
