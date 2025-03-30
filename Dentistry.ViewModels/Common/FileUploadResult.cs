using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.ViewModels.Common
{
    public class FileUploadResult
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ThumbPath { get; set; }
        public long? FileSize { get; set; }
    }
}
