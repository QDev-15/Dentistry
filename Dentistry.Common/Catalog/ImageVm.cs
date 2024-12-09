using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.ViewModels.Catalog
{
    public class ImageVm
    {
        public int Id { get; set; }

        public string Path { get; set; }
        public string Type { get; set; }
        public long FileSize { get; set; }
    }
}
