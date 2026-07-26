using Dentistry.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Services
{
    public class AppConfigService
    {
        public readonly HostingConfig HostingConfig;
        public readonly JwtTokens JwtTokens;
        public readonly int DefaultLanguageId;
        public readonly ConnectionStrings ConnectionStrings;
        public AppConfigService(IConfiguration configuration)
        {
            HostingConfig = configuration.GetSection("HostingConfig").Get<HostingConfig>();
            JwtTokens = configuration.GetSection("JwtTokens").Get<JwtTokens>();
            DefaultLanguageId = configuration.GetValue<int>("DefaultLanguageId");
            ConnectionStrings = configuration.GetSection("ConnectionStrings").Get<ConnectionStrings>();
        }

    }
    public class ConnectionStrings
    {
        public string DentistryDbConnection { get; set; }
    }
}
