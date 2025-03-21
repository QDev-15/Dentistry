using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.ViewModels.Catalog.Categories
{
    public class CategoryBox
    {
        public List<CategoryVm> Items = new List<CategoryVm>();
        public string Title { get; set; } = null;
        public string SubTitle { set; get; } = null;
    }
}
