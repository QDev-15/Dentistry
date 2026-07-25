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
            text = text ?? "";
            text = text.Trim();

            text = text.ConvertToSlug();
            // Chuyển thành chữ thường
            text = text.ToLowerInvariant();

            // "đ" không được NormalizationForm.FormD tách dấu nên phải xử lý riêng
            text = text.Replace("đ", "d");

            // Loại bỏ dấu
            text = RemoveDiacritics(text);

            // Loại bỏ ký tự không phải chữ, số, khoảng trắng hoặc gạch ngang
            text = Regex.Replace(text, @"[^a-z0-9\s-]", "");

            // Gộp khoảng trắng và các gạch ngang liên tiếp thành một gạch ngang
            text = Regex.Replace(text, @"[\s-]+", "-");

            // Xóa gạch ngang thừa ở đầu/cuối
            text = text.Trim('-');

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
