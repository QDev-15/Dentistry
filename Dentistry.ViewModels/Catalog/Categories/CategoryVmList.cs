using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.ViewModels.Catalog.Categories
{
    public class CategoryVmList
    {
        public List<CategoryVm> Parents { get; set; } = new List<CategoryVm>();
        public List<CategoryVm> Childs { get; set; } = new List<CategoryVm>();
    }
}
