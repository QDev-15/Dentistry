using Dentistry.ViewModels.Catalog.AppSettings;
using Dentisty.Data.GeneratorDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Interfaces
{
    public interface IAppSettingRepository : IRepository<AppSetting>
    {
        Task<AppSettingVm> Update(AppSettingVm appSettingVm);
    }
}
