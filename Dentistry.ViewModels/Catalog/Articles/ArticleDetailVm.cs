using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.ViewModels.Catalog.Articles
{
    public class ArticleDetailVm
    {
        public ArticleVm item { set; get; } = new ArticleVm();
        public List<ArticleVm> items { set; get; }  = new List<ArticleVm>();
    }
}
