using Azure.Core;
using Dentistry.Data.GeneratorDB.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dentisty.Data.Common
{
    public static class Utilities
    {
        public static string HashPassword(string password)
        {
            var hasher = new PasswordHasher<object>(); // Use object because it needs a class type, but it won't be used.
            var hashedPassword = hasher.HashPassword(null, password); // null is for the user object, since we are not using it here
            return hashedPassword;
        }
        public static async Task<bool> VerifyPassword(UserManager<AppUser> userManager, AppUser user, string enteredPassword)
        {
            var result = await userManager.CheckPasswordAsync(user, enteredPassword);
            return result; // Returns true if the password is correct, false otherwise
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
