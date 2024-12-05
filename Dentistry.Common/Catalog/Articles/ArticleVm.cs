using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.ViewModels.Catalog.Articles
{
    public class ArticleVm
    {
        public int Id { get; set; }
        public string Alias { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int SortOrder { get; set; } = 0;
        public bool showHome { get; set; } = false;
        public long FileSize { get; set; }
        public string Url { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
