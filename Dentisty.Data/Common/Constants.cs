using System;
using System.Collections.Generic;
using System.Text;

namespace Dentistry.Common.Constants
{
    public class Constants
    {
        public const string MainConnectionString = "DentistryDbConnection";
        public const string CartSession = "CartSession";
        public const string NA = "N/A";
        public const string USER_CONTENT_FOLDER_NAME = "user-content";
        public class AppSettings
        {
            public const string DefaultLanguageId = "DefaultLanguageId";
            public const string Token = "Token";
            public const string AuthToken = "AuthToken";
            public const string BaseAddress = "BaseAddress";
        }
        public static class JwtTokens {
            public const string Key = "JwtTokens:Key";
            public const string Issuer = "JwtTokens:Issuer";
            public const string Audience = "JwtTokens:Audience";
            public const string ExpiresInMinutes = "JwtTokens:ExpiresInMinutes";
        }
        
    }
}