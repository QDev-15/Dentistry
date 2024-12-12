using Dentisty.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data
{
    public static class TypeExtensions
    {
        public static string ToSlus(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return Utilities.ConvertToSlug(value);
        }
    }
}
