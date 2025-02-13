using Dentistry.ViewModels.Catalog.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.ViewModels.Catalog.Home
{
    public class SearchVm
    {
        public List<ArticleVm> Items { set; get; } = new List<ArticleVm>();
        public List<ArticleVm> HotNews { set; get; } = new List<ArticleVm>();

    }
}
