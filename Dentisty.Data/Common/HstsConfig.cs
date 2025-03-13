using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Common
{
    public class HstsConfig
    {
        public int MaxAge { set; get; }
        public bool IncludeSubDomains { set; get; }
        public bool Preload { set; get; }
    }
}
