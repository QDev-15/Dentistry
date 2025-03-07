using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.ViewModels.Catalog.Branches
{
    public class BranchesVm
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string? Code { set; get; }
        public string Address { set; get; }
        public string PhoneNumber { set; get; }
        public bool IsActive { set; get; }
        public DateTime CreatedAt { set; get; }
        public DateTime UpdatedAt { set; get; }

        public string GetDisplayHtml() {
            return Name + "</br> " + Address + " | <span style=\"font-weight: bold;\">" + PhoneNumber + "</span>";
        }
    }
}
