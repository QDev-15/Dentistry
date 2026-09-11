using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Dentisty.Data.Services.Email
{
    // Encrypts/decrypts the SMTP password stored on AppSetting using AES with a key shared
    // between Dentistry.Admin and Dentistry.Web (config key "SmtpEncryption:Key").
    //
    // This intentionally does NOT use ASP.NET Core's Data Protection API: Data Protection
    // isolates its key ring per application by default, so a value protected by the Admin app
    // cannot be unprotected by the Web app (they are two separate deployed applications) unless
    // both are additionally configured with a matching SetApplicationName + a shared key storage
    // location - which is fragile across different hosting setups (separate app pools, servers,
    // OSes). A plain shared-secret AES key avoids that dependency entirely.
    public class SmtpCredentialProtector : ISmtpCredentialProtector
    {
        private readonly byte[] _key;

        public SmtpCredentialProtector(IConfiguration configuration)
        {
            var base64Key = configuration["SmtpEncryption:Key"];
            if (string.IsNullOrWhiteSpace(base64Key))
            {
                throw new InvalidOperationException("Thiếu cấu hình \"SmtpEncryption:Key\" trong appsettings.json.");
            }
            _key = Convert.FromBase64String(base64Key);
        }

        public string? Protect(string? plaintext)
        {
            if (string.IsNullOrEmpty(plaintext))
            {
                return plaintext;
            }

            using var aes = Aes.Create();
            aes.Key = _key;
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor();
            var plainBytes = Encoding.UTF8.GetBytes(plaintext);
            var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            // Store IV + ciphertext together so Unprotect only needs the one string back.
            var combined = new byte[aes.IV.Length + cipherBytes.Length];
            Buffer.BlockCopy(aes.IV, 0, combined, 0, aes.IV.Length);
            Buffer.BlockCopy(cipherBytes, 0, combined, aes.IV.Length, cipherBytes.Length);
            return Convert.ToBase64String(combined);
        }

        public string? Unprotect(string? protectedText)
        {
            if (string.IsNullOrEmpty(protectedText))
            {
                return protectedText;
            }

            try
            {
                var combined = Convert.FromBase64String(protectedText);
                using var aes = Aes.Create();
                aes.Key = _key;

                var ivLength = aes.BlockSize / 8;
                var iv = new byte[ivLength];
                Buffer.BlockCopy(combined, 0, iv, 0, ivLength);
                aes.IV = iv;

                using var decryptor = aes.CreateDecryptor();
                var cipherLength = combined.Length - ivLength;
                var plainBytes = decryptor.TransformFinalBlock(combined, ivLength, cipherLength);
                return Encoding.UTF8.GetString(plainBytes);
            }
            catch (Exception)
            {
                // Corrupted value, wrong key, or a value from before this scheme existed -
                // treat as unset rather than throwing, so a bad password never crashes sending.
                return null;
            }
        }
    }
}
