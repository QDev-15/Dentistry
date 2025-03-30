using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dentistry.ViewModels
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Example: abc-def-123
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToSlus(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return ConvertToSlug(value);
        }
        /// <summary>
        /// Example: abc_def_123
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToSlus_V2(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return ConvertToSlug(value).Replace('-', '_');
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
        public static string ConvertToSlug(string text)
        {
            // Chuyển thành chữ thường
            text = text.ToLowerInvariant();

            // Loại bỏ dấu
            text = RemoveDiacritics(text);

            // Thay thế khoảng trắng bằng dấu gạch ngang
            text = Regex.Replace(text, @"\s+", "-");

            return text;
        }
        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var character in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(character);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(character);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

    }
}
