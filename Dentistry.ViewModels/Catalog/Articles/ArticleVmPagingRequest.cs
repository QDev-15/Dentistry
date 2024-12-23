using Dentistry.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dentistry.ViewModels.Catalog.Articles

{
    public class ArticleVmPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }

        public int? CategoryId { get; set; }
    }
}