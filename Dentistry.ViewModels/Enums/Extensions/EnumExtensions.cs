using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;


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
