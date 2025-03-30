using Dentistry.ViewModels.Catalog.Articles;
using Dentistry.ViewModels.Catalog.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.ViewModels.Catalog.Categories
{
    public class CategoryDetailVm
    {
        public BookFormVm bookFormVm { set; get; }  = new BookFormVm();
        public CategoryVm category { set; get; }  = new CategoryVm();
        public List<ArticleVm> articles { set; get; } = new List<ArticleVm>();
        public List<ArticleVm> hotNews { set; get; } = new List<ArticleVm>();
    }
}
