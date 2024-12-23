using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Common
{
    public class HostingConfig
    {
        public string Domain { get; set; }
        public string UploadDirectory { get; set; }
        public List<string> AllowedExtensions { get; set; }
        public int MaxFileSizeMB { get; set; }
    }
}
