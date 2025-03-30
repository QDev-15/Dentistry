using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.ViewModels.Catalog.Logger
{
    public class LoggerResult
    {
        public List<LoggerVm> Items { get; set; } = new List<LoggerVm>();
        public int Total { get; set; } = 0;
    }
}
