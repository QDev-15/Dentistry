using Dentistry.ViewModels.Catalog.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.ViewModels.Catalog.Articles
{
    public class ArticleVmAddEdit
    {
        public ArticleVm Item {  get; set; }
        public List<CategoryVm> Categories { get; set; } = new List<CategoryVm>();
    }
}
