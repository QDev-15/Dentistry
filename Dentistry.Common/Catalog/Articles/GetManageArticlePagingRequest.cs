using Dentistry.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dentistry.ViewModels.Catalog.Articles

{
    public class GetManageArticlePagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }

        public int? LanguageId { get; set; }

        public int? CategoryId { get; set; }
    }
}