using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dentistry.ViewModels.System.Users
{
    public class LoginRequest
    {
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
        public string TimeZone { set; get; }
        public string IpAddress { set; get; }
    }
}