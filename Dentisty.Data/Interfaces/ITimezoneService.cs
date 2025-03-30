using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Interfaces
{
    public interface ITimezoneService
    {
        string GetUserTimeZoneId();
        TimeZoneInfo GetUserTimeZone();
    }
}
