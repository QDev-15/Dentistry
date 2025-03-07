using Dentistry.ViewModels.Catalog.Branches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.ViewModels.Catalog.Contacts
{
    public class BookFormVm
    {
        public ContactVm contact { set; get; } = new ContactVm();
        public List<BranchesVm> branches { set; get; } = new List<BranchesVm>();
        public bool SupportView { set; get; } = false;
    }
}
