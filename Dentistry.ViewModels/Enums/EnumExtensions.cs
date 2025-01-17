using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

using System.Net.WebSockets;


namespace Dentistry.ViewModels.Enums
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum? enumValue, string defaultValue = "Không xác định")
        {
            if (enumValue == null) { return defaultValue; }
            var displayAttribute = enumValue.GetType()
                                             .GetField(enumValue.ToString()!)
                                             .GetCustomAttributes(typeof(DisplayAttribute), false)
                                             .FirstOrDefault() as DisplayAttribute;

            return displayAttribute != null ? displayAttribute.Name : enumValue.ToString()!;
        }
        public static string GetAliasDisplayName(this Enum? enumValue, string defaultValue = "Không xác định")
        {
            if (enumValue == null) { return defaultValue; }
            var displayAttribute = enumValue.GetType()
                                             .GetField(enumValue.ToString()!)
                                             .GetCustomAttributes(typeof(DisplayAttribute), false)
                                             .FirstOrDefault() as DisplayAttribute;

            string value = displayAttribute != null ? displayAttribute.GroupName : enumValue.ToString()!;
            value = value.ToSlus();
            if (!string.IsNullOrEmpty(value))
            {
                value = displayAttribute != null ? displayAttribute.Name : enumValue.ToString()!;
                value = value.ToSlus();
            }
            return value;

        }
        public static List<SelectListItem> ToSelectList<TEnum>() where TEnum : struct, Enum
        {
            return Enum.GetValues(typeof(TEnum))
                       .Cast<TEnum>()
                       .Select(e => new SelectListItem
                       {
                           Value = Convert.ToInt32(e).ToString(),
                           Text = e.GetDisplayName()
                       }).ToList();
        }
    }

}
