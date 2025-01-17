using Dentistry.ViewModels.Catalog.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.ViewModels.Catalog.Categories
{
    public class CategoryDetailVm
    {
        public CategoryVm item { set; get; }  = new CategoryVm();
        public List<ArticleVm> articles { set; get; } = new List<ArticleVm>();
    }
}
