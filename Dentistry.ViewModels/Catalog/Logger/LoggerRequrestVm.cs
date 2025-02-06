using Dentistry.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.ViewModels.Catalog.Logger
{
    public class LoggerRequrestVm : PagingRequestBase
    {
        public string keySearch { set; get; }
        public string SortColumn { get; set; } = "createdDate";
        public string SortDirection { get; set; } = "desc"; // Mặc định sắp xếp giảm dần
    }
}
