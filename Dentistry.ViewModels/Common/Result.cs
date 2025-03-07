using System;
using System.Collections.Generic;
using System.Text;

namespace Dentistry.ViewModels.Common
{
    public class Result<T>
    {
        public bool IsSuccessed { get; set; }

        public string Message { get; set; }
        public object data { get; set; }

        public T ResultObj { get; set; }
    }
}