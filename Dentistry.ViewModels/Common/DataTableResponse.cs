using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.ViewModels.Common
{
    public class DataTableResponse<T>
    {
        public int Draw { set; get; }
        public int RecordsTotal { set; get; }
        public int RecordsFiltered { set; get; }
        public List<T> Data { set; get; }
    }
}
