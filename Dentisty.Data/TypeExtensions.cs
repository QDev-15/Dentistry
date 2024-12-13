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
        /// <summary>
        ///  dd/MM/yyyy - 15/09/2009
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Format_DMY(this DateTime value)
        {
            return value.ToString("d");
        }
        public static string GetTimestamp(this DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }
    }
}
